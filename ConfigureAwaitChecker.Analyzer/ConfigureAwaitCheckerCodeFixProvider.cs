using System;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using ConfigureAwaitChecker.Lib;

namespace ConfigureAwaitChecker.Analyzer
{
	[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ConfigureAwaitCheckerCodeFixProvider)), Shared]
	public sealed class ConfigureAwaitCheckerCodeFixProvider : CodeFixProvider
	{
		public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(ConfigureAwaitCheckerAnalyzer.MissingConfigureAwaitFalseId, ConfigureAwaitCheckerAnalyzer.ConfigureAwaitWithTrueId);

		public override FixAllProvider GetFixAllProvider()
		{
			return WellKnownFixAllProviders.BatchFixer;
		}

		public override async Task RegisterCodeFixesAsync(CodeFixContext context)
		{
			var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
			var diagnostic = context.Diagnostics.First();
			if (root.FindNode(diagnostic.Location.SourceSpan, getInnermostNodeForTie: true) is AwaitExpressionSyntax node)
			{
				context.RegisterCodeFix(
					CodeAction.Create("Correct to `ConfigureAwait(false)`", c => Fix(context.Document, node, c), equivalenceKey: nameof(ConfigureAwaitCheckerCodeFixProvider)),
					diagnostic);
			}
		}

		static async Task<Document> Fix(Document document, AwaitExpressionSyntax node, CancellationToken cancellationToken)
		{
			var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
			var (expression, invocation) = Checker.FindExpressionForConfigureAwait(node);
			if (invocation != null)
			{
				if (!Checker.IsConfigureAwait(invocation.Expression))
				{
					var falseExpression = SyntaxFactory.LiteralExpression(SyntaxKind.FalseLiteralExpression);
					var newExpression = SyntaxFactory.InvocationExpression(
						SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, invocation, SyntaxFactory.IdentifierName(Checker.ConfigureAwaitIdentifier)),
						SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[] { SyntaxFactory.Argument(falseExpression) })));
					return document.WithSyntaxRoot(root.ReplaceNode(invocation, newExpression.WithAdditionalAnnotations(Formatter.Annotation)));
				}
				if (!Checker.HasFalseArgument(invocation.ArgumentList))
				{
					var falseExpression = SyntaxFactory.LiteralExpression(SyntaxKind.FalseLiteralExpression);
					var newExpression = SyntaxFactory.InvocationExpression(invocation.Expression,
						SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[] { SyntaxFactory.Argument(falseExpression) })));
					return document.WithSyntaxRoot(root.ReplaceNode(invocation, newExpression.WithAdditionalAnnotations(Formatter.Annotation)));
				}
			}
			else if (expression != null)
			{
#warning Should properly handle "await using"
				var e = expression;
				if (e.IsKind(SyntaxKind.AwaitExpression))
				{
					e = SyntaxFactory.ParenthesizedExpression(e);
				}
				var falseExpression = SyntaxFactory.LiteralExpression(SyntaxKind.FalseLiteralExpression);
				var newExpression = SyntaxFactory.InvocationExpression(
					SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, e, SyntaxFactory.IdentifierName(Checker.ConfigureAwaitIdentifier)),
					SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[] { SyntaxFactory.Argument(falseExpression) })));
				return document.WithSyntaxRoot(root.ReplaceNode(expression, newExpression.WithAdditionalAnnotations(Formatter.Annotation)));
			}
			else
			{
				var e = node.Expression;
				if (e.IsKind(SyntaxKind.CastExpression))
				{
					e = SyntaxFactory.ParenthesizedExpression(e);
				}
				var falseExpression = SyntaxFactory.LiteralExpression(SyntaxKind.FalseLiteralExpression);
				var newExpression = SyntaxFactory.InvocationExpression(
					SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, e, SyntaxFactory.IdentifierName(Checker.ConfigureAwaitIdentifier)),
					SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[] { SyntaxFactory.Argument(falseExpression) })));
				return document.WithSyntaxRoot(root.ReplaceNode(node.Expression, newExpression.WithAdditionalAnnotations(Formatter.Annotation)));
			}
			throw new InvalidOperationException();
		}
	}
}
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
		public override ImmutableArray<string> FixableDiagnosticIds
		{
			get { return ImmutableArray.Create(ConfigureAwaitCheckerAnalyzer.DiagnosticId); }
		}

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
					CodeAction.Create("Correct to `ConfigureAwait(false)`", c => Fix(context.Document, node, c)),
					diagnostic);
			}
		}

		static async Task<Document> Fix(Document document, AwaitExpressionSyntax node, CancellationToken cancellationToken)
		{
			var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
			var expression = Checker.FindExpressionForConfigureAwait(node);
			if (expression != null)
			{
				if (!Checker.IsConfigureAwait(expression.Expression))
				{
					var falseExpression = SyntaxFactory.LiteralExpression(SyntaxKind.FalseLiteralExpression);
					var newExpression = SyntaxFactory.InvocationExpression(
						SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, expression, SyntaxFactory.IdentifierName(Checker.ConfigureAwaitIdentifier)),
						SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[] { SyntaxFactory.Argument(falseExpression) })));
					return document.WithSyntaxRoot(root.ReplaceNode(expression, newExpression.WithAdditionalAnnotations(Formatter.Annotation)));
				}
				if (!Checker.HasFalseArgument(expression.ArgumentList))
				{
					var falseExpression = SyntaxFactory.LiteralExpression(SyntaxKind.FalseLiteralExpression);
					var newExpression = SyntaxFactory.InvocationExpression(expression.Expression,
						SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[] { SyntaxFactory.Argument(falseExpression) })));
					return document.WithSyntaxRoot(root.ReplaceNode(expression, newExpression.WithAdditionalAnnotations(Formatter.Annotation)));
				}
			}
			else
			{
				var falseExpression = SyntaxFactory.LiteralExpression(SyntaxKind.FalseLiteralExpression);
				var newExpression = SyntaxFactory.InvocationExpression(
					SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, node.Expression, SyntaxFactory.IdentifierName(Checker.ConfigureAwaitIdentifier)),
					SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[] { SyntaxFactory.Argument(falseExpression) })));
				return document.WithSyntaxRoot(root.ReplaceNode(node.Expression, newExpression.WithAdditionalAnnotations(Formatter.Annotation)));
			}
			throw new InvalidOperationException();
		}
	}
}
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
		const string Title = "Correct to `ConfigureAwait(false)`";

		public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(ConfigureAwaitCheckerAnalyzer.MissingConfigureAwaitFalseId, ConfigureAwaitCheckerAnalyzer.ConfigureAwaitWithTrueId);

		public override FixAllProvider GetFixAllProvider()
		{
			return WellKnownFixAllProviders.BatchFixer;
		}

		public override async Task RegisterCodeFixesAsync(CodeFixContext context)
		{
			var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
			var diagnostic = context.Diagnostics.First();
			var node = root.FindNode(diagnostic.Location.SourceSpan, getInnermostNodeForTie: true);
			switch (node)
			{
				case AwaitExpressionSyntax awaitNode:
					context.RegisterCodeFix(
						CodeAction.Create(Title, c => Fix(context.Document, awaitNode, c), equivalenceKey: nameof(ConfigureAwaitCheckerCodeFixProvider)),
						diagnostic);
					break;
				case UsingStatementSyntax usingNode:
					context.RegisterCodeFix(
						CodeAction.Create(Title, c => Fix(context.Document, usingNode, c), equivalenceKey: nameof(ConfigureAwaitCheckerCodeFixProvider)),
						diagnostic);
					break;
				case LocalDeclarationStatementSyntax localDeclarationNode:
					context.RegisterCodeFix(
						CodeAction.Create(Title, c => Fix(context.Document, localDeclarationNode, c), equivalenceKey: nameof(ConfigureAwaitCheckerCodeFixProvider)),
						diagnostic);
					break;
				case ForEachStatementSyntax forEachNode:
					context.RegisterCodeFix(
						CodeAction.Create(Title, c => Fix(context.Document, forEachNode, c), equivalenceKey: nameof(ConfigureAwaitCheckerCodeFixProvider)),
						diagnostic);
					break;
			}
		}

		static async Task<Document> Fix(Document document, AwaitExpressionSyntax node, CancellationToken cancellationToken)
		{
			var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
			var (invocation, expression) = Checker.FindNodeFor(node);
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
			else
			{
				var falseExpression = SyntaxFactory.LiteralExpression(SyntaxKind.FalseLiteralExpression);
				var newExpression = SyntaxFactory.InvocationExpression(
					SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, PrepareNode(expression), SyntaxFactory.IdentifierName(Checker.ConfigureAwaitIdentifier)),
					SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[] { SyntaxFactory.Argument(falseExpression) })));
				return document.WithSyntaxRoot(root.ReplaceNode(expression, newExpression.WithAdditionalAnnotations(Formatter.Annotation)));

				static ExpressionSyntax PrepareNode(ExpressionSyntax expression)
				{
					if (expression.IsKind(SyntaxKind.CastExpression))
						return SyntaxFactory.ParenthesizedExpression(expression);
					return expression;
				}
			}
			throw new InvalidOperationException();
		}

		static async Task<Document> Fix(Document document, UsingStatementSyntax node, CancellationToken cancellationToken)
		{
#warning Adding ConfigureAwait might result in different variable type and hence should be probably extracted outside
			var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
			var (invocation, expression) = Checker.FindNodeFor(node);
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
			else
			{
				var falseExpression = SyntaxFactory.LiteralExpression(SyntaxKind.FalseLiteralExpression);
				var newExpression = SyntaxFactory.InvocationExpression(
					SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, expression, SyntaxFactory.IdentifierName(Checker.ConfigureAwaitIdentifier)),
					SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[] { SyntaxFactory.Argument(falseExpression) })));
				return document.WithSyntaxRoot(root.ReplaceNode(expression, newExpression.WithAdditionalAnnotations(Formatter.Annotation)));
			}
			throw new InvalidOperationException();
		}

		static async Task<Document> Fix(Document document, LocalDeclarationStatementSyntax node, CancellationToken cancellationToken)
		{
#warning Adding ConfigureAwait might result in different variable type and hence should be probably extracted outside
			var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
			var (invocation, expression) = Checker.FindNodeFor(node);
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
			else
			{
				var falseExpression = SyntaxFactory.LiteralExpression(SyntaxKind.FalseLiteralExpression);
				var newExpression = SyntaxFactory.InvocationExpression(
					SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, expression, SyntaxFactory.IdentifierName(Checker.ConfigureAwaitIdentifier)),
					SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[] { SyntaxFactory.Argument(falseExpression) })));
				return document.WithSyntaxRoot(root.ReplaceNode(expression, newExpression.WithAdditionalAnnotations(Formatter.Annotation)));
			}
			throw new InvalidOperationException();
		}

		static async Task<Document> Fix(Document document, ForEachStatementSyntax node, CancellationToken cancellationToken)
		{
			var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
			var (invocation, expression) = Checker.FindNodeFor(node);
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
			else
			{
				var falseExpression = SyntaxFactory.LiteralExpression(SyntaxKind.FalseLiteralExpression);
				var newExpression = SyntaxFactory.InvocationExpression(
					SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, expression, SyntaxFactory.IdentifierName(Checker.ConfigureAwaitIdentifier)),
					SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[] { SyntaxFactory.Argument(falseExpression) })));
				return document.WithSyntaxRoot(root.ReplaceNode(expression, newExpression.WithAdditionalAnnotations(Formatter.Annotation)));
			}
			throw new InvalidOperationException();
		}
	}
}
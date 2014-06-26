using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConfigureAwaitChecker.Lib
{
	public sealed class Checker
	{
		static readonly CSharpParseOptions ParseOptions = new CSharpParseOptions(
				languageVersion: LanguageVersion.CSharp5,
				documentationMode: DocumentationMode.None,
				kind: SourceCodeKind.Regular);

		SyntaxTree _tree;

		public Checker(string file)
		{
			_tree = CSharpSyntaxTree.ParseFile(file,
				options: ParseOptions);
		}

		public IEnumerable<CheckerResult> Check()
		{
			foreach (var item in _tree.GetRoot().DescendantNodesAndTokens())
			{
				if (item.CSharpKind() == SyntaxKind.AwaitExpression)
				{
					var awaitNode = item.AsNode();
					var line = awaitNode.GetLocation().GetMappedLineSpan();
					var possibleConfigureAwait = FindExpressionForConfigureAwait(awaitNode);
					var good = possibleConfigureAwait != null && IsProperConfigureAwait(possibleConfigureAwait);
					yield return new CheckerResult(good, line.StartLinePosition.Line, line.StartLinePosition.Character);
				}
			}
		}

		public string DebugListTree()
		{
			return DebugListNodes(_tree.GetRoot().ChildNodes());
		}

		static InvocationExpressionSyntax FindExpressionForConfigureAwait(SyntaxNode node)
		{
			foreach (var item in node.ChildNodes())
			{
				if (item is InvocationExpressionSyntax)
					return (InvocationExpressionSyntax)item;
				return FindExpressionForConfigureAwait(item);
			}
			return null;
		}

		static bool IsProperConfigureAwait(InvocationExpressionSyntax invocationExpression)
		{
			return IsConfigureAwait(invocationExpression.Expression) && HasFalseArgument(invocationExpression.ArgumentList);
		}

		static bool IsConfigureAwait(ExpressionSyntax expression)
		{
			var memberAccess = expression as MemberAccessExpressionSyntax;
			if (memberAccess == null)
				return false;
			if (!memberAccess.Name.Identifier.Text.Equals("ConfigureAwait", StringComparison.InvariantCulture))
				return false;
			return true;
		}

		static bool HasFalseArgument(ArgumentListSyntax argumentList)
		{
			if (argumentList.Arguments.Count != 1)
				return false;
			if (argumentList.Arguments[0].Expression.CSharpKind() != SyntaxKind.FalseLiteralExpression)
				return false;
			return true;
		}

		static string DebugListNodes(IEnumerable<SyntaxNode> nodes, string indent = "")
		{
			var result = new StringBuilder();
			foreach (var node in nodes)
			{
				result.AppendFormat("{0}{1}:[{3}]|{2}",
					indent,
					node.CSharpKind(),
					node.ToString(),
					node.Span);
				result.AppendLine();
				result.Append(DebugListNodes(node.ChildNodes(), indent + "  "));
			}
			return result.ToString();
		}
	}
}

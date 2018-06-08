using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConfigureAwaitChecker.Lib
{
	public sealed class Checker
	{
		public static readonly string ConfigureAwaitIdentifier = "ConfigureAwait";

		static readonly CSharpParseOptions ParseOptions = new CSharpParseOptions(
				languageVersion: LanguageVersion.CSharp6,
				documentationMode: DocumentationMode.None,
				kind: SourceCodeKind.Regular);

		SyntaxTree _tree;

		public Checker(Stream file)
		{
			using (var reader = new StreamReader(file, Encoding.UTF8, true, 16 * 1024, true))
			{
				_tree = CSharpSyntaxTree.ParseText(reader.ReadToEnd(),
					options: ParseOptions);
			}
		}

		public IEnumerable<CheckerResult> Check()
		{
			foreach (var item in _tree.GetRoot().DescendantNodesAndTokens())
			{
				if (item.IsKind(SyntaxKind.AwaitExpression))
				{
					var awaitNode = (AwaitExpressionSyntax)item.AsNode();
					yield return CheckNode(awaitNode);
				}
			}
		}

		public static CheckerResult CheckNode(AwaitExpressionSyntax awaitNode)
		{
			var possibleConfigureAwait = FindExpressionForConfigureAwait(awaitNode);
			var good = possibleConfigureAwait != null && IsConfigureAwait(possibleConfigureAwait.Expression) && HasFalseArgument(possibleConfigureAwait.ArgumentList);
			var needs = !good;
			return new CheckerResult(needs, awaitNode.GetLocation());
		}

		public static InvocationExpressionSyntax FindExpressionForConfigureAwait(SyntaxNode node)
		{
			foreach (var item in node.ChildNodes())
			{
				if (item is InvocationExpressionSyntax)
					return (InvocationExpressionSyntax)item;
				return FindExpressionForConfigureAwait(item);
			}
			return null;
		}

		public static bool IsConfigureAwait(ExpressionSyntax expression)
		{
			var memberAccess = expression as MemberAccessExpressionSyntax;
			if (memberAccess == null)
				return false;
			if (!memberAccess.Name.Identifier.Text.Equals(ConfigureAwaitIdentifier, StringComparison.Ordinal))
				return false;
			return true;
		}

		public static bool HasFalseArgument(ArgumentListSyntax argumentList)
		{
			if (argumentList.Arguments.Count != 1)
				return false;
			if (!argumentList.Arguments[0].Expression.IsKind(SyntaxKind.FalseLiteralExpression))
				return false;
			return true;
		}
	}
}

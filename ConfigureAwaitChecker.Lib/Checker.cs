using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConfigureAwaitChecker.Lib
{
	public sealed class Checker
	{
		public static readonly string ConfigureAwaitIdentifier = "ConfigureAwait";

		static readonly CSharpParseOptions ParseOptions = new CSharpParseOptions(
				languageVersion: LanguageVersion.Latest,
				documentationMode: DocumentationMode.None,
				kind: SourceCodeKind.Regular);

		readonly MetadataReference[] _references;

		public Checker(params MetadataReference[] references)
		{
			_references = references;
		}

		public IEnumerable<CheckerResult> Check(Stream file)
		{
			var tree = ParseFile(file);
			var compilation = CSharpCompilation.Create(nameof(ConfigureAwaitChecker));
			compilation = compilation.AddReferences(_references);
			compilation = compilation.AddSyntaxTrees(tree);
			var semanticModel = compilation.GetSemanticModel(tree);
			foreach (var item in tree.GetRoot().DescendantNodes())
			{
				if (item.IsKind(SyntaxKind.AwaitExpression))
				{
					var awaitNode = (AwaitExpressionSyntax)item;
					yield return CheckNode(awaitNode, semanticModel);
				}
			}
		}

		public static CheckerResult CheckNode(AwaitExpressionSyntax awaitNode, SemanticModel semanticModel)
		{
			var possibleConfigureAwait = FindExpressionForConfigureAwait(awaitNode);
			if (possibleConfigureAwait != null && IsConfigureAwait(possibleConfigureAwait.Expression))
			{
				return new CheckerResult(awaitNode.GetLocation()) { NeedsSwitchConfigureAwaitToFalse = !HasFalseArgument(possibleConfigureAwait.ArgumentList) };
			}
			else
			{
				return new CheckerResult(awaitNode.GetLocation()) { NeedsAddConfigureAwaitFalse = CanHaveConfigureAwait(awaitNode.Expression, semanticModel) };
			}
		}

		public static InvocationExpressionSyntax FindExpressionForConfigureAwait(SyntaxNode node)
		{
			foreach (var item in node.ChildNodes())
			{
				if (item is InvocationExpressionSyntax invocationExpressionSyntax)
					return invocationExpressionSyntax;
				return FindExpressionForConfigureAwait(item);
			}
			return null;
		}

		public static bool CanHaveConfigureAwait(ExpressionSyntax expression, SemanticModel semanticModel)
		{
			var typeInfo = semanticModel.GetTypeInfo(expression);
			var type = typeInfo.ConvertedType;
			if (type == null)
				return false;
			var members = type.GetMembers(ConfigureAwaitIdentifier);
			foreach (var item in members)
			{
				if (!(item is IMethodSymbol methodSymbol))
					break;
				var parameters = methodSymbol.Parameters;
				if (parameters.Length != 1)
					break;
				if (parameters[0].Type.SpecialType != SpecialType.System_Boolean)
					break;

				return true;
			}
			return false;
		}

		public static bool IsConfigureAwait(ExpressionSyntax expression)
		{
			if (!(expression is MemberAccessExpressionSyntax memberAccess))
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

		SyntaxTree ParseFile(Stream file)
		{
			using (var reader = new StreamReader(file, Encoding.UTF8, true, 16 * 1024, true))
			{
				return CSharpSyntaxTree.ParseText(reader.ReadToEnd(),
					options: ParseOptions);
			}
		}
	}
}

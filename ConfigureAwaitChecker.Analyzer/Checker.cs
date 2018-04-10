using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConfigureAwaitChecker.Analyzer
{
	public sealed class Checker
	{
		public static readonly string ConfigureAwaitIdentifier = "ConfigureAwait";

		static readonly CSharpParseOptions ParseOptions = new CSharpParseOptions(
				languageVersion: LanguageVersion.CSharp6,
				documentationMode: DocumentationMode.None,
				kind: SourceCodeKind.Regular);

		SyntaxTree _tree;
		private CSharpCompilation _compilation;

		public Checker(Stream file, IReadOnlyList<string> referenceLocations)
		{
			_compilation = CSharpCompilation.Create("ConfigureAwaitCheck");

			foreach (var referenceLocation in referenceLocations)
			{
				_compilation = _compilation.AddReferences(MetadataReference.CreateFromFile(referenceLocation));
			}

			_tree = AddFile(file);
		}

		public SyntaxTree AddFile(Stream file)
		{
			using (var reader = new StreamReader(file, Encoding.UTF8, true, 16 * 1024, true))
			{
				var tree = CSharpSyntaxTree.ParseText(reader.ReadToEnd(),
					options: ParseOptions);

				_compilation = _compilation.AddSyntaxTrees(tree);

				return tree;
			}
		}

		public IEnumerable<CheckerResult> Check()
		{
			var semanticModel = _compilation.GetSemanticModel(_tree);

			foreach (var item in _tree.GetRoot().DescendantNodesAndTokens())
			{
				if (item.IsKind(SyntaxKind.AwaitExpression))
				{
					var awaitNode = (AwaitExpressionSyntax)item.AsNode();
					yield return CheckNode(awaitNode, semanticModel);
				}
			}
		}

		public static CheckerResult CheckNode(AwaitExpressionSyntax awaitNode, SemanticModel semanticModel)
		{
			// Try to find ConfigureAwait in syntax tree without use semantic model first
			if (HasConfigureAwait(awaitNode))
			{
				return new CheckerResult(true, awaitNode.GetLocation());
			}

			var canConfigureAwait = IsConfigureAwaitExpression(awaitNode.Expression, semanticModel);

			return new CheckerResult(!canConfigureAwait, awaitNode.GetLocation());
		}

		public static bool HasConfigureAwait(AwaitExpressionSyntax awaitNode)
		{
			var expression = FindExpressionForConfigureAwait(awaitNode);
			if (expression == null)
				return false;

			var memberAccess = expression.Expression as MemberAccessExpressionSyntax;
			if (memberAccess == null)
				return false;

			if (!memberAccess.Name.Identifier.Text.Equals(ConfigureAwaitIdentifier, StringComparison.Ordinal))
				return false;

			return HasBoolArgument(expression.ArgumentList);
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

		public static bool IsConfigureAwaitExpression(ExpressionSyntax expression, SemanticModel semanticModel)
		{
			if (semanticModel == null) throw new ArgumentNullException(nameof(semanticModel));

			var expressionType = semanticModel.GetTypeInfo(expression);

			if (expressionType.Type == null)
			{
				return false;
			}

			return expressionType.Type.GetMembers(ConfigureAwaitIdentifier).Length > 0;
		}

		public static bool HasBoolArgument(ArgumentListSyntax argumentList)
		{
			if (argumentList.Arguments.Count != 1)
				return false;

			var expression = argumentList.Arguments[0].Expression;

			return expression.IsKind(SyntaxKind.FalseLiteralExpression) ||
			       expression.IsKind(SyntaxKind.TrueLiteralExpression);
		}
	}
}

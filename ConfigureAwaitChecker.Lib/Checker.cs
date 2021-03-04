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

		readonly MetadataReference[] _references;
		readonly LanguageVersion _languageVersion;

		public Checker(LanguageVersion languageVersion, params MetadataReference[] references)
		{
			_references = references;
			_languageVersion = languageVersion;
		}

		public IEnumerable<CheckerResult> Check(Stream file)
		{
			var tree = ParseFile(file, _languageVersion);
			var compilation = CSharpCompilation.Create(nameof(ConfigureAwaitChecker));
			compilation = compilation.AddReferences(_references);
			compilation = compilation.AddSyntaxTrees(tree);
			var semanticModel = compilation.GetSemanticModel(tree);
			foreach (var item in tree.GetRoot().DescendantNodes())
			{
				switch (item)
				{
					case AwaitExpressionSyntax awaitNode:
						yield return CheckNode(awaitNode, semanticModel);
						break;
					case UsingStatementSyntax usingNode:
						var usingNodeResult = CheckNode(usingNode, semanticModel);
						if (usingNodeResult != null)
							yield return usingNodeResult;
						break;
					case ForEachStatementSyntax forEachNode:
						var forEachNodeResult = CheckNode(forEachNode, semanticModel);
						if (forEachNodeResult != null)
							yield return forEachNodeResult;
						break;

				}
			}

			static SyntaxTree ParseFile(Stream file, LanguageVersion languageVersion)
			{
				using (var reader = new StreamReader(file, Encoding.UTF8, true, 16 * 1024, true))
				{
					return CSharpSyntaxTree.ParseText(reader.ReadToEnd(),
						options: new CSharpParseOptions(
							languageVersion: languageVersion,
							documentationMode: DocumentationMode.None,
							kind: SourceCodeKind.Regular));
				}
			}
		}

		public static CheckerResult CheckNode(AwaitExpressionSyntax awaitNode, SemanticModel semanticModel)
		{
			var location = awaitNode.GetLocation();
			var (possibleConfigureAwait, node) = FindExpressionFor(awaitNode);
			if (possibleConfigureAwait != null && IsConfigureAwait(possibleConfigureAwait.Expression))
			{
				if (HasFalseArgument(possibleConfigureAwait.ArgumentList))
				{
					return new CheckerResult(CheckerProblem.NoProblem, location);
				}
				else
				{
					return new CheckerResult(CheckerProblem.ConfigureAwaitWithTrue, location);
				}
			}
			else
			{
				var can = CanHaveConfigureAwait(node, semanticModel);
				var problem = can
					? CheckerProblem.MissingConfigureAwaitFalse
					: CheckerProblem.NoProblem;
				return new CheckerResult(problem, location);
			}
		}

		public static (InvocationExpressionSyntax, ExpressionSyntax) FindExpressionFor(AwaitExpressionSyntax awaitNode)
		{
			static (InvocationExpressionSyntax, ExpressionSyntax) Helper(SyntaxNode node, AwaitExpressionSyntax awaitNode)
			{
				foreach (var item in node.ChildNodes())
				{
					if (item is InvocationExpressionSyntax invocationExpressionSyntax)
						return (invocationExpressionSyntax, awaitNode.Expression);
					return Helper(item, awaitNode);
				}
				return (null, awaitNode.Expression);
			}

			return Helper(awaitNode, awaitNode);
		}

		public static CheckerResult CheckNode(UsingStatementSyntax usingNode, SemanticModel semanticModel)
		{
			var location = usingNode.GetLocation();
			if (usingNode.AwaitKeyword == default)
				return null;
			var (possibleConfigureAwait, node) = FindExpressionFor(usingNode);
			if (possibleConfigureAwait != null && IsConfigureAwait(possibleConfigureAwait.Expression))
			{
				if (HasFalseArgument(possibleConfigureAwait.ArgumentList))
				{
					return new CheckerResult(CheckerProblem.NoProblem, location);
				}
				else
				{
					return new CheckerResult(CheckerProblem.ConfigureAwaitWithTrue, location);
				}
			}
			else
			{
				var can = CanHaveConfigureAwait(node, semanticModel);
				var problem = can
					? CheckerProblem.MissingConfigureAwaitFalse
					: CheckerProblem.NoProblem;
				return new CheckerResult(problem, location);
			}
		}

		public static (InvocationExpressionSyntax, ExpressionSyntax) FindExpressionFor(UsingStatementSyntax usingNode)
		{
			if (usingNode.Declaration != null)
			{
#warning Should be extended to handle all variables
				var initializer = usingNode.Declaration.Variables.First().Initializer.Value;
				if (initializer is InvocationExpressionSyntax invocationExpressionSyntax)
					return (invocationExpressionSyntax, initializer);
				return (null, initializer);
			}
			else
			{
				var expression = usingNode.Expression;
				if (expression is InvocationExpressionSyntax invocationExpressionSyntax)
					return (invocationExpressionSyntax, expression);
				return (null, expression);
			}
		}

		public static CheckerResult CheckNode(ForEachStatementSyntax forEachNode, SemanticModel semanticModel)
		{
			var location = forEachNode.GetLocation();
			if (forEachNode.AwaitKeyword == default)
				return null;
			var (possibleConfigureAwait, node) = FindExpressionFor(forEachNode);
			if (possibleConfigureAwait != null && IsConfigureAwait(possibleConfigureAwait.Expression))
			{
				if (HasFalseArgument(possibleConfigureAwait.ArgumentList))
				{
					return new CheckerResult(CheckerProblem.NoProblem, location);
				}
				else
				{
					return new CheckerResult(CheckerProblem.ConfigureAwaitWithTrue, location);
				}
			}
			else
			{
				var can = CanHaveConfigureAwait(node, semanticModel);
				var problem = can
					? CheckerProblem.MissingConfigureAwaitFalse
					: CheckerProblem.NoProblem;
				return new CheckerResult(problem, location);
			}
		}

		public static (InvocationExpressionSyntax, ExpressionSyntax) FindExpressionFor(ForEachStatementSyntax forEachNode)
		{
			var expression = forEachNode.Expression;
			if (expression is InvocationExpressionSyntax invocationExpressionSyntax)
				return (invocationExpressionSyntax, expression);
			return (null, expression);
		}

		public static bool CanHaveConfigureAwait(ExpressionSyntax expression, SemanticModel semanticModel)
		{
			var typeInfo = semanticModel.GetTypeInfo(expression);
			var type = typeInfo.ConvertedType;
			if (type == null)
				return false;
			var members = semanticModel.LookupSymbols(expression.SpanStart, type, ConfigureAwaitIdentifier, true);
			foreach (var item in members)
			{
				if (!(item is IMethodSymbol methodSymbol))
					continue;
				var parameters = methodSymbol.Parameters;
				if (parameters.Length != 1)
					continue;
				if (parameters[0].Type.SpecialType != SpecialType.System_Boolean)
					continue;

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
	}
}

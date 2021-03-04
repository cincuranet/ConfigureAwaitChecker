using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
				if (item.IsKind(SyntaxKind.AwaitExpression))
				{
					var awaitNode = (AwaitExpressionSyntax)item;
					yield return CheckNode(awaitNode, semanticModel);
				}
			}
		}

		public static CheckerResult CheckNode(AwaitExpressionSyntax awaitNode, SemanticModel semanticModel)
		{
			var location = awaitNode.GetLocation();
			var (expression, invocation) = FindExpressionForConfigureAwait(awaitNode);
			if (expression != null && IsConfigureAwait(expression))
			{
				if (HasFalseArgument(invocation.ArgumentList))
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
				var can = CanHaveConfigureAwait(invocation ?? expression ?? awaitNode.Expression, semanticModel);
				var problem = can
					? CheckerProblem.MissingConfigureAwaitFalse
					: CheckerProblem.NoProblem;
				return new CheckerResult(problem, location);
			}
		}

		public static (ExpressionSyntax, InvocationExpressionSyntax) FindExpressionForConfigureAwait(SyntaxNode node)
		{
			var fullParent = node.Parent.Parent.ChildNodes().ToList();
			if (fullParent.Count == 2 && fullParent[0] == node.Parent)
			{
				if (fullParent[1] is ForEachStatementSyntax forEachStatementSyntax)
				{
					var f = forEachStatementSyntax.ChildNodes().Skip(1).First();
					if (f is InvocationExpressionSyntax e1)
					{
						return (e1.Expression, e1);
					}
					else if (f is AwaitExpressionSyntax e2)
					{
						return (e2, null);
					}
					return (null, null);
				}
				if (fullParent[1] is UsingStatementSyntax usingStatementSyntax)
				{
					var u = usingStatementSyntax.ChildNodes().First();
					if (u is VariableDeclarationSyntax e1)
					{
#warning Should be extended to handle all variables
						foreach (var variable in e1.Variables)
						{
							if (variable.Initializer.Value is InvocationExpressionSyntax invocationExpressionSyntax)
								return (invocationExpressionSyntax.Expression, invocationExpressionSyntax);
							if (variable.Initializer.Value is AwaitExpressionSyntax awaitExpressionSyntax)
								return (awaitExpressionSyntax, null);
						}
					}
					else if (u is InvocationExpressionSyntax e2)
					{
						return (e2.Expression, e2);
					}
					else if (u is AwaitExpressionSyntax e3)
					{
						return (e3, null);
					}
					return (null, null);
				}
			}
			foreach (var item in node.ChildNodes())
			{
				if (item is InvocationExpressionSyntax invocationExpressionSyntax)
					return (invocationExpressionSyntax.Expression, invocationExpressionSyntax);
				return FindExpressionForConfigureAwait(item);
			}
			return (null, null);
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
}

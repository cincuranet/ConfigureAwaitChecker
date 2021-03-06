using System.Collections.Immutable;
using ConfigureAwaitChecker.Lib;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ConfigureAwaitChecker.Analyzer
{
	[DiagnosticAnalyzer(LanguageNames.CSharp)]
	public sealed class ConfigureAwaitCheckerAnalyzer : DiagnosticAnalyzer
	{
		const string Title = "ConfigureAwaitChecker";
		const string Category = "Code";

		public const string MissingConfigureAwaitFalseId = "CAC001";
		public const string ConfigureAwaitWithTrueId = "CAC002";

		static readonly DiagnosticDescriptor MissingConfigureAwaitFalseRule = new DiagnosticDescriptor(MissingConfigureAwaitFalseId, Title, "Possibly missing `ConfigureAwait(false)` call", Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);
		static readonly DiagnosticDescriptor ConfigureAwaitWithTrueRule = new DiagnosticDescriptor(ConfigureAwaitWithTrueId, Title, "Possibly wrong `ConfigureAwait(true)` call", Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);

		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(MissingConfigureAwaitFalseRule, ConfigureAwaitWithTrueRule);

		public override void Initialize(AnalysisContext context)
		{
			context.EnableConcurrentExecution();
			context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
			context.RegisterSyntaxNodeAction(AnalyzeAwait, SyntaxKind.AwaitExpression);
			context.RegisterSyntaxNodeAction(AnalyzeUsing, SyntaxKind.UsingStatement);
			context.RegisterSyntaxNodeAction(AnalyzeLocalDeclaration, SyntaxKind.LocalDeclarationStatement);
			context.RegisterSyntaxNodeAction(AnalyzeForEach, SyntaxKind.ForEachStatement);
		}

		static void AnalyzeAwait(SyntaxNodeAnalysisContext context)
		{
			var node = (AwaitExpressionSyntax)context.Node;
			var check = Checker.CheckNode(node, context.SemanticModel);
			ProcessResult(context, check);
		}

		static void AnalyzeUsing(SyntaxNodeAnalysisContext context)
		{
			var node = (UsingStatementSyntax)context.Node;
			var check = Checker.CheckNode(node, context.SemanticModel);
			ProcessResult(context, check);
		}

		static void AnalyzeLocalDeclaration(SyntaxNodeAnalysisContext context)
		{
			var node = (LocalDeclarationStatementSyntax)context.Node;
			var check = Checker.CheckNode(node, context.SemanticModel);
			ProcessResult(context, check);
		}

		static void AnalyzeForEach(SyntaxNodeAnalysisContext context)
		{
			var node = (ForEachStatementSyntax)context.Node;
			var check = Checker.CheckNode(node, context.SemanticModel);
			ProcessResult(context, check);
		}

		static void ProcessResult(SyntaxNodeAnalysisContext context, CheckerResult check)
		{
			if (check == null)
				return;
			if (check.NeedsFix)
			{
				switch (check.Problem)
				{
					case CheckerProblem.NoProblem:
						return;
					case CheckerProblem.MissingConfigureAwaitFalse:
						context.ReportDiagnostic(Diagnostic.Create(MissingConfigureAwaitFalseRule, check.Location));
						return;
					case CheckerProblem.ConfigureAwaitWithTrue:
						context.ReportDiagnostic(Diagnostic.Create(ConfigureAwaitWithTrueRule, check.Location));
						return;
				}
			}
		}
	}
}

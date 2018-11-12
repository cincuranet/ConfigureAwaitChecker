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
		private const string Title = "ConfigureAwaitChecker";
		public const string AddConfigureAwaitFalseDiagnosticId = "CAC001";
		public const string SwitchToConfigureAwaitFalseDiagnosticId = "CAC002";

		private const string Category = "Code";

		static readonly DiagnosticDescriptor AddConfigureAwaitFalseRule = new DiagnosticDescriptor(AddConfigureAwaitFalseDiagnosticId, Title, "Possibly missing `ConfigureAwait(false)` call", Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);
		static readonly DiagnosticDescriptor SwitchToConfigureAwaitFalseRule = new DiagnosticDescriptor(SwitchToConfigureAwaitFalseDiagnosticId, Title, "Possibly should switch to `ConfigureAwait(false)`", Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);

		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(AddConfigureAwaitFalseRule, SwitchToConfigureAwaitFalseRule); } }

		public override void Initialize(AnalysisContext context)
		{
			context.RegisterSyntaxNodeAction(Analyze, SyntaxKind.AwaitExpression);
		}

		static void Analyze(SyntaxNodeAnalysisContext context)
		{
			var awaitNode = (AwaitExpressionSyntax)context.Node;
			var check = Checker.CheckNode(awaitNode, context.SemanticModel);
			if (check.NeedsAddConfigureAwaitFalse)
			{
				var diagnostic = Diagnostic.Create(AddConfigureAwaitFalseRule, check.Location);
				context.ReportDiagnostic(diagnostic);
			}

			if (check.NeedsSwitchConfigureAwaitToFalse)
			{
				var diagnostic = Diagnostic.Create(SwitchToConfigureAwaitFalseRule, check.Location);
				context.ReportDiagnostic(diagnostic);
			}
		}
	}
}

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace ConfigureAwaitChecker.Analyzer
{
	[DiagnosticAnalyzer(LanguageNames.CSharp)]
	public sealed class ConfigureAwaitCheckerAnalyzer : DiagnosticAnalyzer
	{
		public const string DiagnosticId = "ConfigureAwaitChecker";

		static readonly string Title = "CAC001";
		static readonly string MessageFormat = "Possibly missing `ConfigureAwait(false)` call";
		const string Category = "Code";

		static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);

		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

		public override void Initialize(AnalysisContext context)
		{
			context.RegisterSyntaxNodeAction(Analyze, SyntaxKind.AwaitExpression);
		}

		static void Analyze(SyntaxNodeAnalysisContext context)
		{
			var awaitNode = (AwaitExpressionSyntax)context.Node;
			var check = Checker.CheckNode(awaitNode);
            if (!check.HasConfigureAwait)
			{
				var diagnostic = Diagnostic.Create(Rule, check.Location);
				context.ReportDiagnostic(diagnostic);
			}
		}
	}
}

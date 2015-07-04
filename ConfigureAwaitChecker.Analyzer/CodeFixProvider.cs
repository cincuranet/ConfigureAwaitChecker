using System;
using System.Collections.Generic;
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
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;

namespace ConfigureAwaitChecker.Analyzer
{
	[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ConfigureAwaitCheckerCodeFixProvider)), Shared]
	public sealed class ConfigureAwaitCheckerCodeFixProvider : CodeFixProvider
	{
		public override ImmutableArray<string> FixableDiagnosticIds
		{
			get { return ImmutableArray.Create(ConfigureAwaitCheckerAnalyzer.DiagnosticId); }
		}

		public override FixAllProvider GetFixAllProvider()
		{
			return WellKnownFixAllProviders.BatchFixer;
		}

		public override async Task RegisterCodeFixesAsync(CodeFixContext context)
		{
#warning Not done yet
		}
	}
}
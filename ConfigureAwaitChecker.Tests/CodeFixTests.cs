using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using ConfigureAwaitChecker.Analyzer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using NUnit.Framework;

namespace ConfigureAwaitChecker.Tests
{
	public class CodeFixTests : TestsBase
	{
		[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
		public sealed class TestThisAttribute : Attribute
		{ }

		[TestCaseSource(nameof(TestSource))]
		public void Test(Type testClass)
		{
			var location = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TestClasses", $"{testClass.Name}.cs");
			TestCode(File.ReadAllText(location));
		}

		static IEnumerable<TestCaseData> TestSource()
		{
			var baseType = typeof(TestsBase);
			var types = baseType.Assembly.GetTypes();
			return types
				.Select(x => (type: x, result: x.GetCustomAttribute<TestThisAttribute>()))
				.Where(x => x.result != null)
				.Select(x => new TestCaseData(x.type));
		}

		void TestCode(string code)
		{
			var analyzer = new ConfigureAwaitCheckerAnalyzer();
			var codeFixProvider = new ConfigureAwaitCheckerCodeFixProvider();

			var document = CreateDocument(code);

			var compilerDiagnostics = GetCompilerDiagnostics(document);
			var analyzerDiagnostics = GetSortedDiagnosticsFromDocuments(analyzer, document);
			foreach (var analyzerDiagnostic in analyzerDiagnostics)
			{
				var actions = new List<CodeAction>();
				var context = new CodeFixContext(document, analyzerDiagnostic, (a, d) => actions.Add(a), CancellationToken.None);
				codeFixProvider.RegisterCodeFixesAsync(context).Wait();

				if (!actions.Any())
				{
					Assert.Fail("No code fixes created.");
				}

				document = ApplyFix(document, actions.Single());
			}

			var newCompilerDiagnostics = GetNewDiagnostics(compilerDiagnostics, GetCompilerDiagnostics(document));
			var newAnalyzerDiagnostics = GetSortedDiagnosticsFromDocuments(analyzer, document);

			if (newCompilerDiagnostics.Any())
			{
				foreach (var d in newCompilerDiagnostics)
				{
					TestContext.WriteLine(d.GetMessage());
				}
				Assert.Fail("Fix resulted in compiler diagnostics.");
			}

			if (newAnalyzerDiagnostics.Any())
			{
				Assert.Fail("Still having own diagnostics.");
			}
		}

		#region Helpers

		Document CreateDocument(params string[] sources)
		{
			var projectId = ProjectId.CreateNewId(debugName: "TestProject");

			var solution = new AdhocWorkspace()
				.CurrentSolution
				.AddProject(projectId, "TestProject", "TestProject", LanguageNames.CSharp)
				.WithProjectParseOptions(projectId, new CSharpParseOptions(LanguageVersion.Latest))
				.AddMetadataReferences(projectId, MetadataReferences);

			var count = 0;
			foreach (var source in sources)
			{
				var newFileName = $"Test{count}.cs";
				var documentId = DocumentId.CreateNewId(projectId, debugName: newFileName);
				solution = solution.AddDocument(documentId, newFileName, SourceText.From(source));
				count++;
			}

			return solution.GetProject(projectId).Documents.First();
		}

		static Document ApplyFix(Document document, CodeAction codeAction)
		{
			var operations = codeAction.GetOperationsAsync(CancellationToken.None).Result;
			var solution = operations.OfType<ApplyChangesOperation>().Single().ChangedSolution;
			return solution.GetDocument(document.Id);
		}

		static IList<Diagnostic> GetSortedDiagnosticsFromDocuments(DiagnosticAnalyzer analyzer, params Document[] documents)
		{
			var projects = new HashSet<Project>();
			foreach (var document in documents)
			{
				projects.Add(document.Project);
			}

			var diagnostics = new List<Diagnostic>();
			foreach (var project in projects)
			{
				var compilationWithAnalyzers = project.GetCompilationAsync().Result.WithAnalyzers(ImmutableArray.Create(analyzer));
				var ds = compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync().Result;
				foreach (var d in ds)
				{
					if (d.Location == Location.None || d.Location.IsInMetadata)
					{
						diagnostics.Add(d);
					}
					else
					{
						for (var i = 0; i < documents.Length; i++)
						{
							var document = documents[i];
							var tree = document.GetSyntaxTreeAsync().Result;
							if (tree == d.Location.SourceTree)
							{
								diagnostics.Add(d);
							}
						}
					}
				}
			}

			diagnostics = diagnostics.OrderBy(d => d.Location.SourceSpan.Start).ToList();
			return diagnostics;
		}

		static IEnumerable<Diagnostic> GetCompilerDiagnostics(Document document)
		{
			return document.GetSemanticModelAsync().Result.GetDiagnostics();
		}

		static IEnumerable<Diagnostic> GetNewDiagnostics(IEnumerable<Diagnostic> diagnostics, IEnumerable<Diagnostic> newDiagnostics)
		{
			var oldArray = diagnostics.OrderBy(d => d.Location.SourceSpan.Start).ToArray();
			var newArray = newDiagnostics.OrderBy(d => d.Location.SourceSpan.Start).ToArray();

			var oldIndex = 0;
			var newIndex = 0;

			while (newIndex < newArray.Length)
			{
				if (oldIndex < oldArray.Length && oldArray[oldIndex].Id == newArray[newIndex].Id)
				{
					++oldIndex;
					++newIndex;
				}
				else
				{
					yield return newArray[newIndex++];
				}
			}
		}

		#endregion
	}
}

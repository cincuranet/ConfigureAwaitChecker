using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ConfigureAwaitChecker.Lib;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using NUnit.Framework;

namespace ConfigureAwaitChecker.Tests
{
	public class CheckerTests : TestsBase
	{
		[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
		public sealed class ExpectedResultAttribute : Attribute
		{
			public ExpectedResultAttribute(params CheckerProblem[] result)
			{
				Result = result;
			}

			public CheckerProblem[] Result { get; }
		}

		[TestCaseSource(nameof(TestSource))]
		public CheckerProblem[] Test(Type testClass)
		{
			var location = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TestClasses", $"{testClass.Name}.cs");
			using (var file = File.OpenRead(location))
			{
				var checker = new Checker(LanguageVersion.Latest, MetadataReferences);
				var result = checker.Check(file).ToList();
				TestContext.WriteLine(Dump(result));
				return result.Select(x => x.Problem).ToArray();
			}
		}

		static IEnumerable<TestCaseData> TestSource()
		{
			var baseType = typeof(TestsBase);
			var types = baseType.Assembly.GetTypes();
			return types
				.Select(x => (type: x, result: x.GetCustomAttribute<ExpectedResultAttribute>()))
				.Where(x => x.result != null)
				.Select(x => new TestCaseData(x.type)
				{
					ExpectedResult = x.result.Result
				});
		}

		static string Dump(IEnumerable<CheckerResult> results)
		{
			var sb = new StringBuilder();
			foreach (var item in results)
			{
				var lineSpan = item.Location.GetMappedLineSpan();
				var start = lineSpan.StartLinePosition;
				var end = lineSpan.EndLinePosition;
				static string FormatLocation(LinePosition position) => $"{position.Line}:{position.Character}";
				sb.Append($"Problem: {item.Problem} (Loc: {FormatLocation(start)}-{FormatLocation(end)})");
				sb.AppendLine();
			}
			return sb.ToString();
		}
	}
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests.TestClasses;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using NUnit.Framework;

namespace ConfigureAwaitChecker.Tests
{
	public class CheckerTests
	{
		[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
		public sealed class ExpectedResultAttribute : Attribute
		{
			public ExpectedResultAttribute(bool[] needsAddConfigureAwaitFalseResult, bool[] needsSwitchConfigureAwaitToFalseResult)
			{
				Result = Enumerable.Zip(needsAddConfigureAwaitFalseResult, needsSwitchConfigureAwaitToFalseResult,
					(needAddResult, needSwitchResult) => (needAddResult, needSwitchResult)).ToArray();
			}

			public (bool, bool)[] Result { get; }
		}

		[TestCaseSource(nameof(TestSource))]
		public (bool, bool)[] Test(Type testClass)
		{
			var location = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TestClasses", $"{testClass.Name}.cs");
			using (var file = File.OpenRead(location))
			{
				var checker = CreateChecker(testClass);
				var result = checker.Check(file).ToList();
				TestContext.WriteLine(Dump(result));
				return result.Select(x => (x.NeedsAddConfigureAwaitFalse, x.NeedsSwitchConfigureAwaitToFalse)).ToArray();
			}
		}

		static IEnumerable<TestCaseData> TestSource()
		{
			var baseType = typeof(TestClassBase);
			var types = baseType.Assembly.GetTypes();
			return types
				.Select(x => ((type: x, result: x.GetCustomAttribute<ExpectedResultAttribute>())))
				.Where(x => x.result != null)
				.Select(x => new TestCaseData(x.type)
				{
					ExpectedResult = x.result.Result
				});
		}

		static Checker CreateChecker(Type testClass)
		{
			var references = new[]
			{
				MetadataReference.CreateFromFile(typeof(TestClassBase).Assembly.Location),
				MetadataReference.CreateFromFile(typeof(Task).Assembly.Location),
				MetadataReference.CreateFromFile(typeof(ValueTask).Assembly.Location),
			};
			return new Checker(references);
		}

		static string Dump(IEnumerable<CheckerResult> results)
		{
			var sb = new StringBuilder();
			foreach (var item in results)
			{
				var lineSpan = item.Location.GetMappedLineSpan();
				var start = lineSpan.StartLinePosition;
				var end = lineSpan.EndLinePosition;
				string FormatLocation(LinePosition position) => $"{position.Line}:{position.Character}";
				sb.Append($"Result: {item.NeedsAddConfigureAwaitFalse} (Loc: {FormatLocation(start)}-{FormatLocation(end)})");
				sb.AppendLine();
			}
			return sb.ToString();
		}
	}
}
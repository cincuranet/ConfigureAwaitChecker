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
		static Checker CreateChecker(Type testClass)
		{
			var references = new[]
			{
				MetadataReference.CreateFromFile(typeof(TestClassBase).Assembly.Location),
				MetadataReference.CreateFromFile(typeof(Task).Assembly.Location),
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
				sb.Append($"Result: {item.NeedsConfigureAwaitFalse} ({FormatLocation(start)} - {FormatLocation(end)})");
				sb.AppendLine();
			}
			return sb.ToString();
		}

		[TestCase(typeof(SimpleAwait_Missing), ExpectedResult = new[] { true })]
		[TestCase(typeof(SimpleAwait_Fine), ExpectedResult = new[] { false })]
		[TestCase(typeof(SimpleAwait_WithTrue), ExpectedResult = new[] { true })]
		[TestCase(typeof(SimpleAwaitWithBraces_Missing), ExpectedResult = new[] { true })]
		[TestCase(typeof(SimpleAwaitWithBracesAll_Fine), ExpectedResult = new[] { false })]
		[TestCase(typeof(SimpleAwaitWithBracesTask_Fine), ExpectedResult = new[] { false })]
		[TestCase(typeof(AwaitInIf_Missing), ExpectedResult = new[] { true })]
		[TestCase(typeof(AwaitInIf_Fine), ExpectedResult = new[] { false })]
		[TestCase(typeof(AwaitInUsing_Missing), ExpectedResult = new[] { true })]
		[TestCase(typeof(AwaitInUsing_Fine), ExpectedResult = new[] { false })]
		[TestCase(typeof(CallOnResult_Missing), ExpectedResult = new[] { true })]
		[TestCase(typeof(CallOnResult_Fine), ExpectedResult = new[] { false })]
		[TestCase(typeof(NestedFunctionCalls_MissingAll), ExpectedResult = new[] { true, true })]
		[TestCase(typeof(NestedFunctionCalls_MissingInner), ExpectedResult = new[] { false, true })]
		[TestCase(typeof(NestedFunctionCalls_MissingOuter), ExpectedResult = new[] { true, false })]
		[TestCase(typeof(NestedFunctionCalls_Fine), ExpectedResult = new[] { false, false })]
		[TestCase(typeof(SimpleLambda_Missing), ExpectedResult = new[] { true })]
		[TestCase(typeof(SimpleLambda_Fine), ExpectedResult = new[] { false })]
		[TestCase(typeof(SimpleLambdaWithBraces_Missing), ExpectedResult = new[] { true })]
		[TestCase(typeof(SimpleLambdaWithBraces_Fine), ExpectedResult = new[] { false })]
		[TestCase(typeof(ExecutingAsyncLambda_MissingAll), ExpectedResult = new[] { true, true })]
		[TestCase(typeof(ExecutingAsyncLambda_MissingInner), ExpectedResult = new[] { false, true })]
		[TestCase(typeof(ExecutingAsyncLambda_MissingOuter), ExpectedResult = new[] { true, false })]
		[TestCase(typeof(ExecutingAsyncLambda_Fine), ExpectedResult = new[] { false, false })]
		[TestCase(typeof(AwaitOnAwaiter_Missing), ExpectedResult = new[] { true })]
		[TestCase(typeof(AwaitOnAwaiter_Fine), ExpectedResult = new[] { false })]
		[TestCase(typeof(ThrowAwait_Missing), ExpectedResult = new[] { true })]
		[TestCase(typeof(ThrowAwait_Fine), ExpectedResult = new[] { false })]
		[TestCase(typeof(AwaitTaskYield_Fine), ExpectedResult = new[] { false })]
		public bool[] Test(Type testClass)
		{
			var location = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TestClasses", $"{testClass.Name}.cs");
			using (var file = File.OpenRead(location))
			{
				var checker = CreateChecker(testClass);
				var result = checker.Check(file).ToList();
				TestContext.WriteLine(Dump(result));
				return result.Select(x => x.NeedsConfigureAwaitFalse).ToArray();
			}
		}
	}
}
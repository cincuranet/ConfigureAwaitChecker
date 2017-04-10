﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using ConfigureAwaitChecker.Analyzer;

namespace ConfigureAwaitChecker.Tests
{
	[TestFixture]
	public class CheckerTests
	{
		static Checker CreateChecker(Type testClass)
		{
			var location = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase), "TestClasses", $"{testClass.Name}.cs");
			location = location.Replace(@"file:\", string.Empty);
			using (var file = File.OpenRead(location))
			{
				return new Checker(file);
			}
		}

		static string Dump(IEnumerable<CheckerResult> results)
		{
			var sb = new StringBuilder();
			foreach (var item in results)
			{
				var location = item.Location.GetMappedLineSpan().StartLinePosition;
				sb.Append($"Result:{item.HasConfigureAwaitExpression}\tL:{location.Line,-6}|C:{location.Character}");
				sb.AppendLine();
			}
			return sb.ToString();
		}

		[TestCase(typeof(SimpleAwait_Missing), ExpectedResult = new[] { false })]
		[TestCase(typeof(SimpleAwait_Fine), ExpectedResult = new[] { true })]
		[TestCase(typeof(SimpleAwait_WithTrue), ExpectedResult = new[] { true })]
		[TestCase(typeof(SimpleAwaitWithBraces_Missing), ExpectedResult = new[] { false })]
		[TestCase(typeof(SimpleAwaitWithBracesAll_Fine), ExpectedResult = new[] { true })]
		[TestCase(typeof(SimpleAwaitWithBracesTask_Fine), ExpectedResult = new[] { true })]
		[TestCase(typeof(AwaitInIf_Missing), ExpectedResult = new[] { false })]
		[TestCase(typeof(AwaitInIf_Fine), ExpectedResult = new[] { true })]
		[TestCase(typeof(AwaitInUsing_Missing), ExpectedResult = new[] { false })]
		[TestCase(typeof(AwaitInUsing_Fine), ExpectedResult = new[] { true })]
		[TestCase(typeof(CallOnResult_Missing), ExpectedResult = new[] { false })]
		[TestCase(typeof(CallOnResult_Fine), ExpectedResult = new[] { true })]
		[TestCase(typeof(NestedFunctionCalls_MissingAll), ExpectedResult = new[] { false, false })]
		[TestCase(typeof(NestedFunctionCalls_MissingInner), ExpectedResult = new[] { true, false })]
		[TestCase(typeof(NestedFunctionCalls_MissingOuter), ExpectedResult = new[] { false, true })]
		[TestCase(typeof(NestedFunctionCalls_Fine), ExpectedResult = new[] { true, true })]
		[TestCase(typeof(SimpleLambda_Missing), ExpectedResult = new[] { false })]
		[TestCase(typeof(SimpleLambda_Fine), ExpectedResult = new[] { true })]
		[TestCase(typeof(SimpleLambdaWithBraces_Missing), ExpectedResult = new[] { false })]
		[TestCase(typeof(SimpleLambdaWithBraces_Fine), ExpectedResult = new[] { true })]
		[TestCase(typeof(ExecutingAsyncLambda_MissingAll), ExpectedResult = new[] { false, false })]
		[TestCase(typeof(ExecutingAsyncLambda_MissingInner), ExpectedResult = new[] { true, false })]
		[TestCase(typeof(ExecutingAsyncLambda_MissingOuter), ExpectedResult = new[] { false, true })]
		[TestCase(typeof(ExecutingAsyncLambda_Fine), ExpectedResult = new[] { true, true })]
		[TestCase(typeof(AwaitOnAwaiter_Missing), ExpectedResult = new[] { false })]
		[TestCase(typeof(AwaitOnAwaiter_Fine), ExpectedResult = new[] { true })]
		[TestCase(typeof(ThrowAwait_Missing), ExpectedResult = new[] { false })]
		[TestCase(typeof(ThrowAwait_Fine), ExpectedResult = new[] { true })]
		public bool[] Test(Type testClass)
		{
			var checker = CreateChecker(testClass);
			var result = checker.Check().ToArray();
			Console.WriteLine(Dump(result));
			return result.Select(x => x.HasConfigureAwaitExpression).ToArray();
		}
	}
}

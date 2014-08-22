using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests.TestClasses;
using NUnit.Framework;

namespace ConfigureAwaitChecker.Tests
{
	[TestFixture]
	public class CheckerTests
	{
		static string File(string className)
		{
			var location = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase), "TestClasses", string.Format("{0}.cs", className));
			return location.Replace(@"file:\", string.Empty);
		}

		static Checker CreateChecker<T>() where T : TestClassBase
		{
			return new Checker(File(typeof(T).Name));
		}

		static CheckerResult[] Check<T>() where T : TestClassBase
		{
			var checker = CreateChecker<T>();
			Debug.WriteLine(checker.DebugListTree());
			var result = CreateChecker<T>().Check().ToArray();
			Console.WriteLine(Dump(result));
			return result;
		}

		static string Dump(IEnumerable<CheckerResult> results)
		{
			var sb = new StringBuilder();
			foreach (var item in results)
			{
				sb.AppendFormat("Result:{0}\tL:{1,-6}|C:{2}",
					item.HasConfigureAwaitFalse,
					item.Line,
					item.Column);
				sb.AppendLine();
			}
			return sb.ToString();
		}

		[Test]
		public void SimpleAwait_Missing()
		{
			var result = Check<SimpleAwait_Missing>();
			Assert.IsFalse(result[0].HasConfigureAwaitFalse);
		}
		[Test]
		public void SimpleAwait_Fine()
		{
			var result = Check<SimpleAwait_Fine>();
			Assert.IsTrue(result[0].HasConfigureAwaitFalse);
		}
		[Test]
		public void SimpleAwait_WithTrue()
		{
			var result = Check<SimpleAwait_WithTrue>();
			Assert.IsFalse(result[0].HasConfigureAwaitFalse);
		}

		[Test]
		public void SimpleAwaitWithBraces_Missing()
		{
			var result = Check<SimpleAwaitWithBraces_Missing>();
			Assert.IsFalse(result[0].HasConfigureAwaitFalse);
		}
		[Test]
		public void SimpleAwaitWithBracesAll_Fine()
		{
			var result = Check<SimpleAwaitWithBracesAll_Fine>();
			Assert.IsTrue(result[0].HasConfigureAwaitFalse);
		}
		[Test]
		public void SimpleAwaitWithBracesTask_Fine()
		{
			var result = Check<SimpleAwaitWithBracesTask_Fine>();
			Assert.IsTrue(result[0].HasConfigureAwaitFalse);
		}

		[Test]
		public void AwaitInIf_Missing()
		{
			var result = Check<AwaitInIf_Missing>();
			Assert.IsFalse(result[0].HasConfigureAwaitFalse);
		}
		[Test]
		public void AwaitInIf_Fine()
		{
			var result = Check<AwaitInIf_Fine>();
			Assert.IsTrue(result[0].HasConfigureAwaitFalse);
		}

		[Test]
		public void AwaitInUsing_Missing()
		{
			var result = Check<AwaitInUsing_Missing>();
			Assert.IsFalse(result[0].HasConfigureAwaitFalse);
		}
		[Test]
		public void AwaitInUsing_Fine()
		{
			var result = Check<AwaitInUsing_Fine>();
			Assert.IsTrue(result[0].HasConfigureAwaitFalse);
		}

		[Test]
		public void CallOnResult_Missing()
		{
			var result = Check<CallOnResult_Missing>();
			Assert.IsFalse(result[0].HasConfigureAwaitFalse);
		}
		[Test]
		public void CallOnResult_Fine()
		{
			var result = Check<CallOnResult_Fine>();
			Assert.IsTrue(result[0].HasConfigureAwaitFalse);
		}

		[Test]
		public void NestedFunctionCalls_MissingAll()
		{
			var result = Check<NestedFunctionCalls_MissingAll>();
			Assert.IsFalse(result[0].HasConfigureAwaitFalse);
			Assert.IsFalse(result[1].HasConfigureAwaitFalse);
		}
		[Test]
		public void NestedFunctionCalls_MissingInner()
		{
			var result = Check<NestedFunctionCalls_MissingInner>();
			Assert.IsTrue(result[0].HasConfigureAwaitFalse);
			Assert.IsFalse(result[1].HasConfigureAwaitFalse);
		}
		[Test]
		public void NestedFunctionCalls_MissingOuter()
		{
			var result = Check<NestedFunctionCalls_MissingOuter>();
			Assert.IsFalse(result[0].HasConfigureAwaitFalse);
			Assert.IsTrue(result[1].HasConfigureAwaitFalse);
		}
		[Test]
		public void NestedFunctionCalls_Fine()
		{
			var result = Check<NestedFunctionCalls_Fine>();
			Assert.IsTrue(result[0].HasConfigureAwaitFalse);
			Assert.IsTrue(result[1].HasConfigureAwaitFalse);
		}

		[Test]
		public void SimpleLambda_Missing()
		{
			var result = Check<SimpleLambda_Missing>();
			Assert.IsFalse(result[0].HasConfigureAwaitFalse);
		}
		[Test]
		public void SimpleLambda_Fine()
		{
			var result = Check<SimpleLambda_Fine>();
			Assert.IsTrue(result[0].HasConfigureAwaitFalse);
		}

		[Test]
		public void SimpleLambdaWithBraces_Missing()
		{
			var result = Check<SimpleLambdaWithBraces_Missing>();
			Assert.IsFalse(result[0].HasConfigureAwaitFalse);
		}
		[Test]
		public void SimpleLambdaWithBraces_Fine()
		{
			var result = Check<SimpleLambdaWithBraces_Fine>();
			Assert.IsTrue(result[0].HasConfigureAwaitFalse);
		}

		[Test]
		public void ExecutingAsyncLambda_MissingAll()
		{
			var result = Check<ExecutingAsyncLambda_MissingAll>();
			Assert.IsFalse(result[0].HasConfigureAwaitFalse);
			Assert.IsFalse(result[1].HasConfigureAwaitFalse);
		}
		[Test]
		public void ExecutingAsyncLambda_MissingInner()
		{
			var result = Check<ExecutingAsyncLambda_MissingInner>();
			Assert.IsTrue(result[0].HasConfigureAwaitFalse);
			Assert.IsFalse(result[1].HasConfigureAwaitFalse);
		}
		[Test]
		public void ExecutingAsyncLambda_MissingOuter()
		{
			var result = Check<ExecutingAsyncLambda_MissingOuter>();
			Assert.IsFalse(result[0].HasConfigureAwaitFalse);
			Assert.IsTrue(result[1].HasConfigureAwaitFalse);
		}
		[Test]
		public void ExecutingAsyncLambda_Fine()
		{
			var result = Check<ExecutingAsyncLambda_Fine>();
			Assert.IsTrue(result[0].HasConfigureAwaitFalse);
			Assert.IsTrue(result[1].HasConfigureAwaitFalse);
		}

		[Test]
		public void AwaitOnAwaiter_Missing()
		{
			var result = Check<AwaitOnAwaiter_Missing>();
			Assert.IsFalse(result[0].HasConfigureAwaitFalse);
		}
		[Test]
		public void AwaitOnAwaiter_Fine()
		{
			var result = Check<AwaitOnAwaiter_Fine>();
			Assert.IsTrue(result[0].HasConfigureAwaitFalse);
		}

		[Test]
		public void ThrowAwait_Missing()
		{
			var result = Check<ThrowAwait_Missing>();
			Assert.IsFalse(result[0].HasConfigureAwaitFalse);
		}

		[Test]
		public void ThrowAwait_Fine()
		{
			var result = Check<ThrowAwait_Fine>();
			Assert.IsTrue(result[0].HasConfigureAwaitFalse);
		}
	}
}

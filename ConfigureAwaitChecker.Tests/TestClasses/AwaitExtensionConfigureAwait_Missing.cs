using ConfigureAwaitChecker.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConfigureAwaitChecker.Tests.TestClasses
{
	[CheckerTests.ExpectedResult(CheckerProblem.MissingConfigureAwaitFalse)]
	public class AwaitExtensionConfigureAwait_Missing : TestClassBase
	{
		public async Task FooBar()
		{
			await new Awaitable();
		}

		public class Awaitable
		{
			public TaskAwaiter GetAwaiter() => default;
		}
	}

	internal static class AwaitExtensionConfigureAwait_Missing_AwaitableExtensions
	{
		public static ConfiguredTaskAwaitable ConfigureAwait(this AwaitExtensionConfigureAwait_Missing.Awaitable @this, bool continueOnCapturedContext) => default;
	}
}

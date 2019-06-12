using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;

namespace ConfigureAwaitChecker.Tests.TestClasses
{
	[CheckerTests.ExpectedResult(CheckerProblem.MissingConfigureAwaitFalse)]
	public class AwaitExtensionWrongAndGoodConfigureAwait_Missing : TestClassBase
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

	static class AwaitExtensionWrongAndGoodConfigureAwait_Missing_AwaitableExtensions
	{
		public static ConfiguredTaskAwaitable ConfigureAwait(this AwaitExtensionWrongAndGoodConfigureAwait_Missing.Awaitable @this, int continueOnCapturedContext) => default;
		public static ConfiguredTaskAwaitable ConfigureAwait(this AwaitExtensionWrongAndGoodConfigureAwait_Missing.Awaitable @this, bool continueOnCapturedContext) => default;
	}
}

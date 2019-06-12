using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;

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

	static class AwaitExtensionConfigureAwait_Missing_AwaitableExtensions
	{
		public static ConfiguredTaskAwaitable ConfigureAwait(this AwaitExtensionConfigureAwait_Missing.Awaitable @this, bool continueOnCapturedContext) => default;
	}
}

using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;

namespace ConfigureAwaitChecker.Tests.TestClasses
{
	[CheckerTests.ExpectedResult(CheckerProblem.MissingConfigureAwaitFalse)]
	public class AwaitCustomTypeWrongAndGoodConfigureAwait_Missing : TestClassBase
	{
		public async Task FooBar()
		{
			await new Awaitable();
		}

		public class Awaitable
		{
			public TaskAwaiter GetAwaiter() => default;
			public ConfiguredTaskAwaitable ConfigureAwait(int continueOnCapturedContext) => default;
			public ConfiguredTaskAwaitable ConfigureAwait(bool continueOnCapturedContext) => default;
		}
	}
}

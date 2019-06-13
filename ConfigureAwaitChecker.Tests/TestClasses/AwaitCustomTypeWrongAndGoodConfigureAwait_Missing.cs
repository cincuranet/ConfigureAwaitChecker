using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.MissingConfigureAwaitFalse)]
[CodeFixTests.TestThis]
public class AwaitCustomTypeWrongAndGoodConfigureAwait_Missing
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

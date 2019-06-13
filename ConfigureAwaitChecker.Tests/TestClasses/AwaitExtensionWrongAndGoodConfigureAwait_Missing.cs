using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.MissingConfigureAwaitFalse)]
[CodeFixTests.TestThis]
public class AwaitExtensionWrongAndGoodConfigureAwait_Missing
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

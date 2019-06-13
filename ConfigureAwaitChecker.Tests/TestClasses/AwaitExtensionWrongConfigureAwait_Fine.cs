using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.NoProblem)]
public class AwaitExtensionWrongConfigureAwait_Fine
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

static class AwaitExtensionWrongConfigureAwait_Fine_AwaitableExtensions
{
	public static ConfiguredTaskAwaitable ConfigureAwait(this AwaitExtensionWrongConfigureAwait_Fine.Awaitable @this, int continueOnCapturedContext) => default;
}

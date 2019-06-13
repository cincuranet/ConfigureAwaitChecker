using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.MissingConfigureAwaitFalse)]
[CodeFixTests.TestThis]
public class AwaitExtensionConfigureAwait_Missing
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

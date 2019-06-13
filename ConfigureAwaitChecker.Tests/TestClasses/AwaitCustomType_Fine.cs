using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.NoProblem)]
public class AwaitCustomType_Fine
{
	public async Task FooBar()
	{
		await new Awaitable().ConfigureAwait(false);
	}

	public class Awaitable
	{
		public TaskAwaiter GetAwaiter() => default;
		public ConfiguredTaskAwaitable ConfigureAwait(bool continueOnCapturedContext) => default;
	}
}
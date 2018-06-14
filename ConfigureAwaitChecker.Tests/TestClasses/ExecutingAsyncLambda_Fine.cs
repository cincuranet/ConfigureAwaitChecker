using System;
using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(new[] { false, false })]
public class ExecutingAsyncLambda_Fine : TestClassBase
{
	public async Task FooBar()
	{
		await ((Func<Task>)(async () => await Task.Delay(1).ConfigureAwait(false)))().ConfigureAwait(false);
	}
}

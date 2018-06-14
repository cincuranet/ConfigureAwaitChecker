using System;
using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(new[] { true, true })]
public class ExecutingAsyncLambda_MissingAll : TestClassBase
{
	public async Task FooBar()
	{
		await ((Func<Task>)(async () => await Task.Delay(1)))();
	}
}

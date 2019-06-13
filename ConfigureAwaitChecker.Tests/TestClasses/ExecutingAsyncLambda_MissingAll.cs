using System;
using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.MissingConfigureAwaitFalse, CheckerProblem.MissingConfigureAwaitFalse)]
[CodeFixTests.TestThis]
public class ExecutingAsyncLambda_MissingAll
{
	public async Task FooBar()
	{
		await ((Func<Task>)(async () => await Task.Delay(1)))();
	}
}

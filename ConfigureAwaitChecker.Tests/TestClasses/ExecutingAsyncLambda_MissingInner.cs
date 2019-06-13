using System;
using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.NoProblem, CheckerProblem.MissingConfigureAwaitFalse)]
[CodeFixTests.TestThis]
public class ExecutingAsyncLambda_MissingInner
{
	public async Task FooBar()
	{
		await ((Func<Task>)(async () => await Task.Delay(1)))().ConfigureAwait(false);
	}
}

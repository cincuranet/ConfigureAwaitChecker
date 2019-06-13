using System;
using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.NoProblem, CheckerProblem.NoProblem)]
public class ExecutingAsyncLambda_Fine
{
	public async Task FooBar()
	{
		await ((Func<Task>)(async () => await Task.Delay(1).ConfigureAwait(false)))().ConfigureAwait(false);
	}
}

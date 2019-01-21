using System;
using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(CheckerProblem.NoProblem, CheckerProblem.MissingConfigureAwaitFalse)]
public class ExecutingAsyncLambda_MissingInner : TestClassBase
{
	public async Task FooBar()
	{
		await ((Func<Task>)(async () => await Task.Delay(1)))().ConfigureAwait(false);
	}
}

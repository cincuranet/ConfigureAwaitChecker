using System;
using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.NoProblem)]
public class SimpleLambda_Fine
{
#pragma warning disable 1998
	public async Task FooBar()
#pragma warning restore 1998
	{
		var func = (Func<Task>)(async () => await Task.Delay(1).ConfigureAwait(false));
	}
}

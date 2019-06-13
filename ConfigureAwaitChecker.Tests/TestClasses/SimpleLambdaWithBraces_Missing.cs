using System;
using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.MissingConfigureAwaitFalse)]
[CodeFixTests.TestThis]
public class SimpleLambdaWithBraces_Missing
{
#pragma warning disable 1998
	public async Task FooBar()
#pragma warning restore 1998
	{
		var func = (Func<Task>)(async () => await (Task.Delay(1)));
	}
}

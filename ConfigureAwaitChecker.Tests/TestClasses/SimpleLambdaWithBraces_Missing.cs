using System;
using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(CheckerProblem.MissingConfigureAwaitFalse)]
public class SimpleLambdaWithBraces_Missing : TestClassBase
{
#pragma warning disable 1998
	public async Task FooBar()
#pragma warning restore 1998
	{
		var func = (Func<Task>)(async () => await (Task.Delay(1)));
	}
}

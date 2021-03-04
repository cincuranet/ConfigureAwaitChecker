using System;
using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.MissingConfigureAwaitFalse, CheckerProblem.NoProblem)]
[CodeFixTests.TestThis]
public class AwaitUsingAwaitInsideWithoutVar_Missing
{
	public async Task FooBar()
	{
		await using (await TestsBase.F<IAsyncDisposable>(default).ConfigureAwait(false))
		{ }
	}
}

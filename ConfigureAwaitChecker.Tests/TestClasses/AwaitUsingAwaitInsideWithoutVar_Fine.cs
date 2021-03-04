using System;
using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.NoProblem, CheckerProblem.NoProblem)]
public class AwaitUsingAwaitInsideWithoutVar_Fine
{
	public async Task FooBar()
	{
		await using ((await TestsBase.F<IAsyncDisposable>(default).ConfigureAwait(false)).ConfigureAwait(false))
		{ }
	}
}

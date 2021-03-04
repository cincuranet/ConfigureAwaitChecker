using System;
using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.MissingConfigureAwaitFalse, CheckerProblem.NoProblem)]
[CodeFixTests.TestThis]
public class AwaitUsing4_Missing
{
	public async Task FooBar()
	{
		await using (var _ = await TestsBase.F<IAsyncDisposable>(default).ConfigureAwait(false))
		{ }
	}
}

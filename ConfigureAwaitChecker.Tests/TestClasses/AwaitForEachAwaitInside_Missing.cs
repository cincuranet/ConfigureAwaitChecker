using System.Collections.Generic;
using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.MissingConfigureAwaitFalse, CheckerProblem.NoProblem)]
[CodeFixTests.TestThis]
public class AwaitForEachAwaitInside_Missing
{
	public async Task FooBar()
	{
		await foreach (var item in await TestsBase.F<IAsyncEnumerable<int>>(default).ConfigureAwait(false))
		{ }
	}
}

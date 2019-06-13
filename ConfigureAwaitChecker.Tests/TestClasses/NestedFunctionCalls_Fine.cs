using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.NoProblem, CheckerProblem.NoProblem)]
public class NestedFunctionCalls_Fine
{
	public async Task FooBar()
	{
		await TestsBase.F(await TestsBase.Bool().ConfigureAwait(false)).ConfigureAwait(false);
	}
}

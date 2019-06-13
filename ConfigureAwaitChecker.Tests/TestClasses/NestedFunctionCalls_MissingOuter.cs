using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.MissingConfigureAwaitFalse, CheckerProblem.NoProblem)]
[CodeFixTests.TestThis]
public class NestedFunctionCalls_MissingOuter
{
	public async Task FooBar()
	{
		await TestsBase.F(await TestsBase.Bool().ConfigureAwait(false));
	}
}

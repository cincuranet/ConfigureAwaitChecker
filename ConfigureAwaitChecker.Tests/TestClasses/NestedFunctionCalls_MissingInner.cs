using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(new[] { CheckerProblem.NoProblem, CheckerProblem.MissingConfigureAwaitFalse })]
[CodeFixTests.TestThis]
public class NestedFunctionCalls_MissingInner
{
	public async Task FooBar()
	{
		await TestsBase.F(await TestsBase.Bool()).ConfigureAwait(false);
	}
}

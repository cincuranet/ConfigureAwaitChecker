using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.MissingConfigureAwaitFalse, CheckerProblem.MissingConfigureAwaitFalse)]
[CodeFixTests.TestThis]
public class NestedFunctionCalls_MissingAll
{
	public async Task FooBar()
	{
		await TestsBase.F(await TestsBase.Bool());
	}
}

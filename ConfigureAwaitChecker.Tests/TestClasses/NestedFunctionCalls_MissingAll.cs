using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(CheckerProblem.MissingConfigureAwaitFalse, CheckerProblem.MissingConfigureAwaitFalse)]
public class NestedFunctionCalls_MissingAll : TestClassBase
{
	public async Task FooBar()
	{
		await F(await Bool());
	}
}

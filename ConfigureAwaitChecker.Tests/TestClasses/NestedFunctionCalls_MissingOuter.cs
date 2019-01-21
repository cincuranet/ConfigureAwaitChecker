using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(CheckerProblem.MissingConfigureAwaitFalse, CheckerProblem.NoProblem)]
public class NestedFunctionCalls_MissingOuter : TestClassBase
{
	public async Task FooBar()
	{
		await F(await Bool().ConfigureAwait(false));
	}
}

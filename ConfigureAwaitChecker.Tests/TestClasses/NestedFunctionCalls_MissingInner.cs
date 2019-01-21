using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(new[] { CheckerProblem.NoProblem, CheckerProblem.MissingConfigureAwaitFalse })]
public class NestedFunctionCalls_MissingInner : TestClassBase
{
	public async Task FooBar()
	{
		await F(await Bool()).ConfigureAwait(false);
	}
}

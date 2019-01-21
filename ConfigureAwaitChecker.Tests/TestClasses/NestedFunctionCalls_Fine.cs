using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(CheckerProblem.NoProblem, CheckerProblem.NoProblem)]
public class NestedFunctionCalls_Fine : TestClassBase
{
	public async Task FooBar()
	{
		await F(await Bool().ConfigureAwait(false)).ConfigureAwait(false);
	}
}

using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.NoProblem)]
public class AwaitInIf_Fine
{
	public async Task FooBar()
	{
		if (await TestsBase.Bool().ConfigureAwait(false)) { }
	}
}

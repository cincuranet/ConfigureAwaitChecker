using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.NoProblem)]
public class ThrowAwait_Fine
{
	public async Task FooBar()
	{
		throw await TestsBase.Exception().ConfigureAwait(false);
	}
}

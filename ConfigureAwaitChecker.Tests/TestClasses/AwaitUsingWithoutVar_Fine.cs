using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.NoProblem)]
public class AwaitUsingWithoutVar_Fine
{
	public async Task FooBar()
	{
		await using (TestsBase.AsyncDisposable().ConfigureAwait(false))
		{ }
	}
}

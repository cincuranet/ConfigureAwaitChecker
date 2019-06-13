using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.NoProblem)]
public class AwaitOnAwaiter_Fine
{
	public async Task FooBar()
	{
		var awaiter = TestsBase.F(6);
		await awaiter.ConfigureAwait(false);
	}
}
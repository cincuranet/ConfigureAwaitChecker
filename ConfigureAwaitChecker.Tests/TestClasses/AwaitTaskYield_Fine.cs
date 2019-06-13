using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.NoProblem)]
public class AwaitTaskYield_Fine
{
	public async Task FooBar()
	{
		await Task.Yield();
	}
}

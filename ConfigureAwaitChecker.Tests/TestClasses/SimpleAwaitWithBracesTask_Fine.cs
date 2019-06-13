using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.NoProblem)]
public class SimpleAwaitWithBracesTask_Fine
{
	public async Task FooBar()
	{
		await (Task.Delay(1)).ConfigureAwait(false);
	}
}

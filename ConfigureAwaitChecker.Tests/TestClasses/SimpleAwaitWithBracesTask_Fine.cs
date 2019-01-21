using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(CheckerProblem.NoProblem)]
public class SimpleAwaitWithBracesTask_Fine : TestClassBase
{
	public async Task FooBar()
	{
		await (Task.Delay(1)).ConfigureAwait(false);
	}
}

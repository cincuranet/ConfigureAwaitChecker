using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(CheckerProblem.ConfigureAwaitWithTrue)]
public class SimpleAwait_WithTrue : TestClassBase
{
	public async Task FooBar()
	{
		await Task.Delay(1).ConfigureAwait(true);
	}
}

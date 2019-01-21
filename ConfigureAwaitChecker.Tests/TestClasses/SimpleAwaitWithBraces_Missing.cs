using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(CheckerProblem.MissingConfigureAwaitFalse)]
public class SimpleAwaitWithBraces_Missing : TestClassBase
{
	public async Task FooBar()
	{
		await (Task.Delay(1));
	}
}

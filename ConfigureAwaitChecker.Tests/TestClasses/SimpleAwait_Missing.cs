using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(CheckerProblem.MissingConfigureAwaitFalse)]
public class SimpleAwait_Missing : TestClassBase
{
	public async Task FooBar()
	{
		await Task.Delay(1);
	}
}

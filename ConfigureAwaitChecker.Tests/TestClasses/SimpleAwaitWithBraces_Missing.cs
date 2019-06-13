using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.MissingConfigureAwaitFalse)]
[CodeFixTests.TestThis]
public class SimpleAwaitWithBraces_Missing
{
	public async Task FooBar()
	{
		await (Task.Delay(1));
	}
}

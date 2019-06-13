using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.ConfigureAwaitWithTrue)]
[CodeFixTests.TestThis]
public class SimpleAwait_WithTrue
{
	public async Task FooBar()
	{
		await Task.Delay(1).ConfigureAwait(true);
	}
}

using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.MissingConfigureAwaitFalse)]
[CodeFixTests.TestThis]
public class AwaitUsingWithoutVar_Missing
{
	public async Task FooBar()
	{
		await using (TestsBase.AsyncDisposable())
		{ }
	}
}

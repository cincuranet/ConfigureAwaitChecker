using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.MissingConfigureAwaitFalse)]
[CodeFixTests.TestThis]
public class AwaitForeach_Missing
{
	public async Task FooBar()
	{
		await foreach (var item in TestsBase.AsyncEnumerable())
		{ }
	}
}

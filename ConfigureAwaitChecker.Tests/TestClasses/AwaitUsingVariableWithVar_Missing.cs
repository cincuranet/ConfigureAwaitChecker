using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.MissingConfigureAwaitFalse)]
[CodeFixTests.TestThis]
public class AwaitUsingVariableWithVar_Missing
{
	public async Task FooBar()
	{
		var x = TestsBase.AsyncDisposable();
		await using (var y = x)
		{ }
	}
}

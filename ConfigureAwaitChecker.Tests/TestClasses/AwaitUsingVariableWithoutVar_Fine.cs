using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.NoProblem)]
public class AwaitUsingVariableWithoutVar_Fine
{
	public async Task FooBar()
	{
		var x = TestsBase.AsyncDisposable();
		await using (x.ConfigureAwait(false))
		{ }
	}
}

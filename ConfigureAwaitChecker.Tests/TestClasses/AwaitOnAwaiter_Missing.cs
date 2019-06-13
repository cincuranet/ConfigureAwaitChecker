using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.MissingConfigureAwaitFalse)]
[CodeFixTests.TestThis]
public class AwaitOnAwaiter_Missing
{
	public async Task FooBar()
	{
		var awaiter = TestsBase.F(6);
		await awaiter;
	}
}
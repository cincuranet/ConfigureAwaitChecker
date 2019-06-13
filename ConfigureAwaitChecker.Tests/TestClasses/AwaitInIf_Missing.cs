using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.MissingConfigureAwaitFalse)]
[CodeFixTests.TestThis]
public class AwaitInIf_Missing
{
	public async Task FooBar()
	{
		if (await TestsBase.Bool()) { }
	}
}

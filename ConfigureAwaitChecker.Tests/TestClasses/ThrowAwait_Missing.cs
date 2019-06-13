using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.MissingConfigureAwaitFalse)]
[CodeFixTests.TestThis]
public class ThrowAwait_Missing
{
	public async Task FooBar()
	{
		throw await TestsBase.Exception();
	}
}

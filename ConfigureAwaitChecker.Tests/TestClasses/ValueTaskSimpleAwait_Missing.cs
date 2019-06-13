using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.MissingConfigureAwaitFalse)]
[CodeFixTests.TestThis]
public class ValueTaskSimpleAwait_Missing
{
	public async ValueTask FooBar()
	{
		await TestsBase.ValueTask();
	}
}

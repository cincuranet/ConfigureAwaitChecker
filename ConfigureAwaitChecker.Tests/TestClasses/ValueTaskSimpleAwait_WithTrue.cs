using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.ConfigureAwaitWithTrue)]
[CodeFixTests.TestThis]
public class ValueTaskSimpleAwait_WithTrue
{
	public async ValueTask FooBar()
	{
		await TestsBase.ValueTask().ConfigureAwait(true);
	}
}

using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.NoProblem)]
public class ValueTaskSimpleAwait_Fine
{
	public async ValueTask FooBar()
	{
		await TestsBase.ValueTask().ConfigureAwait(false);
	}
}

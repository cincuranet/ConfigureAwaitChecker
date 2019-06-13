using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.NoProblem)]
public class AwaitInUsing_Fine
{
	public async Task FooBar()
	{
		using (var disposable = await TestsBase.Disposable().ConfigureAwait(false)) { }
	}
}

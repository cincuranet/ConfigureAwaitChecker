using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.MissingConfigureAwaitFalse)]
[CodeFixTests.TestThis]
public class AwaitInUsing_Missing
{
	public async Task FooBar()
	{
		using (var disposable = await TestsBase.Disposable()) { }
	}
}

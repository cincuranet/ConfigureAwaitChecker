using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(CheckerProblem.NoProblem)]
public class AwaitInUsing_Fine : TestClassBase
{
	public async Task FooBar()
	{
		using (var disposable = await Disposable().ConfigureAwait(false)) { }
	}
}

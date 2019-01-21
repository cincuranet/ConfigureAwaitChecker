using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(CheckerProblem.MissingConfigureAwaitFalse)]
public class AwaitInUsing_Missing : TestClassBase
{
	public async Task FooBar()
	{
		using (var disposable = await Disposable()) { }
	}
}

using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(new[] { true })]
public class AwaitInUsing_Missing : TestClassBase
{
	public async Task FooBar()
	{
		using (var disposable = await Disposable()) { }
	}
}

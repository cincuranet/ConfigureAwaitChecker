using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(new[] { false }, new[] { false })]
public class AwaitInUsing_Fine : TestClassBase
{
	public async Task FooBar()
	{
		using (var disposable = await Disposable().ConfigureAwait(false)) { }
	}
}

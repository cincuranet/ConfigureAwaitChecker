using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(new[] { false }, new[] { false })]
public class SimpleAwait_Fine : TestClassBase
{
	public async Task FooBar()
	{
		await Task.Delay(1).ConfigureAwait(false);
	}
}

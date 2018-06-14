using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(new[] { true })]
public class SimpleAwait_WithTrue : TestClassBase
{
	public async Task FooBar()
	{
		await Task.Delay(1).ConfigureAwait(true);
	}
}

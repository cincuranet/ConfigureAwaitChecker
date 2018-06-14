using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(new[] { false })]
public class ThrowAwait_Fine : TestClassBase
{
	public async Task FooBar()
	{
		throw await Exception().ConfigureAwait(false);
	}
}

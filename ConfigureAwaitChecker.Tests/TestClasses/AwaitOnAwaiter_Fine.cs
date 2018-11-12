using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(new[] { false }, new[] { false })]
public class AwaitOnAwaiter_Fine : TestClassBase
{
	public async Task FooBar()
	{
		var awaiter = F(6);
		await awaiter.ConfigureAwait(false);
	}
}
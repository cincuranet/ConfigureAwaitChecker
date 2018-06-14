using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(new[] { false })]
public class AwaitInIf_Fine : TestClassBase
{
	public async Task FooBar()
	{
		if (await Bool().ConfigureAwait(false)) { }
	}
}

using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(new[] { false, false })]
public class NestedFunctionCalls_Fine : TestClassBase
{
	public async Task FooBar()
	{
		await F(await Bool().ConfigureAwait(false)).ConfigureAwait(false);
	}
}

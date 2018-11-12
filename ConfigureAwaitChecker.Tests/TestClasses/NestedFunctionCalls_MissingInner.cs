using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(new[] { false, true }, new[] { false, false })]
public class NestedFunctionCalls_MissingInner : TestClassBase
{
	public async Task FooBar()
	{
		await F(await Bool()).ConfigureAwait(false);
	}
}

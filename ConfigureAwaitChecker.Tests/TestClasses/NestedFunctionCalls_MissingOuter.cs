using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(new[] { true, false }, new[] { false, false })]
public class NestedFunctionCalls_MissingOuter : TestClassBase
{
	public async Task FooBar()
	{
		await F(await Bool().ConfigureAwait(false));
	}
}

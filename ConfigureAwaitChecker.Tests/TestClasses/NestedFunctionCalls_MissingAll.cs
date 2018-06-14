using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(new[] { true, true })]
public class NestedFunctionCalls_MissingAll : TestClassBase
{
	public async Task FooBar()
	{
		await F(await Bool());
	}
}

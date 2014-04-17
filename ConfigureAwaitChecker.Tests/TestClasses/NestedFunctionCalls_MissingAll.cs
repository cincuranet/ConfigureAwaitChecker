using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests.TestClasses;

public class NestedFunctionCalls_MissingAll : TestClassBase
{
	public async Task FooBar()
	{
		await F(await Bool());
	}
}

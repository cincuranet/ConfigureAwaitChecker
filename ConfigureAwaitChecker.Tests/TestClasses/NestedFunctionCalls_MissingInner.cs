using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests.TestClasses;

public class NestedFunctionCalls_MissingInner : TestClassBase
{
	public async Task FooBar()
	{
		await F(await Bool()).ConfigureAwait(false);
	}
}

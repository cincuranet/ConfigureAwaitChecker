using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests.TestClasses;

public class AwaitTaskYield_Fine : TestClassBase
{
	public async Task FooBar()
	{
		await Task.Yield();
	}
}

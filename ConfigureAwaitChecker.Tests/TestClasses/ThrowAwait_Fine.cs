using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests.TestClasses;

public class ThrowAwait_Fine : TestClassBase
{
	public async Task FooBar()
	{
		throw await Exception().ConfigureAwait(false);
	}
}

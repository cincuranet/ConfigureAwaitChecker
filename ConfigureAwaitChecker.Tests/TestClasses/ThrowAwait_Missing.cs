using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests.TestClasses;

public class ThrowAwait_Missing : TestClassBase
{
	public async Task FooBar()
	{
		throw await Exception();
	}
}

using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests.TestClasses;

public class SimpleAwait_Missing : TestClassBase
{
	public async Task FooBar()
	{
		await Task.Delay(1);
	}
}

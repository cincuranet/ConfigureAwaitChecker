using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests.TestClasses;

public class SimpleAwait_Fine : TestClassBase
{
	public async Task FooBar()
	{
		await Task.Delay(1).ConfigureAwait(false);
	}
}

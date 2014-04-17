using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests.TestClasses;

public class SimpleAwaitWithBraces_Missing : TestClassBase
{
	public async Task FooBar()
	{
		await (Task.Delay(1));
	}
}

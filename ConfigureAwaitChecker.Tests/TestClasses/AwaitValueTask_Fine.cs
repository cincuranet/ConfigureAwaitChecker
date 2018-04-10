using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests.TestClasses;

public class AwaitValueTask_Fine : TestClassBase
{
	public async Task FooBar()
	{
		await ValueTask().ConfigureAwait(false);
	}
}

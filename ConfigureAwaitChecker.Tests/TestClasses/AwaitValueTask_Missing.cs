using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests.TestClasses;

public class AwaitValueTask_Missing : TestClassBase
{
	public async Task FooBar()
	{
		await ValueTask();
	}
}

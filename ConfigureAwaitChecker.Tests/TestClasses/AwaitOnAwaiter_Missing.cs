using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests.TestClasses;

public class AwaitOnAwaiter_Missing : TestClassBase
{
	public async Task FooBar()
	{
		var awaiter = F(6);
		await awaiter;
	}
}
using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests.TestClasses;

public class AwaitInIf_Missing : TestClassBase
{
	public async Task FooBar()
	{
		if (await Bool()) { }
	}
}

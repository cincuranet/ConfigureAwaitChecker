using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests.TestClasses;

public class AwaitInIf_Fine : TestClassBase
{
	public async Task FooBar()
	{
		if (await Bool().ConfigureAwait(false)) { }
	}
}

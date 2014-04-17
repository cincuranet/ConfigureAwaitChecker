using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests.TestClasses;

public class AwaitInUsing_Fine : TestClassBase
{
	public async Task FooBar()
	{
		using (var disposable = await Disposable().ConfigureAwait(false)) { }
	}
}

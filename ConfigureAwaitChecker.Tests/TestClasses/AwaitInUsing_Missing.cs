using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests.TestClasses;

public class AwaitInUsing_Missing : TestClassBase
{
	public async Task FooBar()
	{
		using (var disposable = await Disposable()) { }
	}
}

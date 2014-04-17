using System.Linq;
using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests.TestClasses;

public class CallOnResult_Missing : TestClassBase
{
	public async Task FooBar()
	{
		var array = (await Enumerable()).ToArray();
	}
}

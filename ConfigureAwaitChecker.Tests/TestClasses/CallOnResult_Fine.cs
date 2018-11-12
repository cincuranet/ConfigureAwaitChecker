using System.Linq;
using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(new[] { false }, new[] { false })]
public class CallOnResult_Fine : TestClassBase
{
	public async Task FooBar()
	{
		var array = (await Enumerable().ConfigureAwait(false)).ToArray();
	}
}

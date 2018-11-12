using System.Linq;
using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(new[] { true }, new[] { false })]
public class CallOnResult_Missing : TestClassBase
{
	public async Task FooBar()
	{
		var array = (await Enumerable()).ToArray();
	}
}

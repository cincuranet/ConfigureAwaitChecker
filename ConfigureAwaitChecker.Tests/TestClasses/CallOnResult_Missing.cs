using System.Linq;
using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.MissingConfigureAwaitFalse)]
[CodeFixTests.TestThis]
public class CallOnResult_Missing
{
	public async Task FooBar()
	{
		var array = (await TestsBase.Enumerable()).ToArray();
	}
}

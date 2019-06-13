using System.Linq;
using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.NoProblem)]
public class CallOnResult_Fine
{
	public async Task FooBar()
	{
		var array = (await TestsBase.Enumerable().ConfigureAwait(false)).ToArray();
	}
}

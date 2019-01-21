using System.Linq;
using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(CheckerProblem.NoProblem)]
public class CallOnResult_Fine : TestClassBase
{
	public async Task FooBar()
	{
		var array = (await Enumerable().ConfigureAwait(false)).ToArray();
	}
}

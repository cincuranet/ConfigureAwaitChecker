using System.Linq;
using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(CheckerProblem.MissingConfigureAwaitFalse)]
public class CallOnResult_Missing : TestClassBase
{
	public async Task FooBar()
	{
		var array = (await Enumerable()).ToArray();
	}
}

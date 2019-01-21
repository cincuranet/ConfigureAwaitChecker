using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(CheckerProblem.NoProblem)]
public class AwaitInIf_Fine : TestClassBase
{
	public async Task FooBar()
	{
		if (await Bool().ConfigureAwait(false)) { }
	}
}

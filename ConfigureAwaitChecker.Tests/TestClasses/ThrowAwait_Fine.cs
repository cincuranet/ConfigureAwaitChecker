using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(CheckerProblem.NoProblem)]
public class ThrowAwait_Fine : TestClassBase
{
	public async Task FooBar()
	{
		throw await Exception().ConfigureAwait(false);
	}
}

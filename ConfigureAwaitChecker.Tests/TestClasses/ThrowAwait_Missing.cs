using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(CheckerProblem.MissingConfigureAwaitFalse)]
public class ThrowAwait_Missing : TestClassBase
{
	public async Task FooBar()
	{
		throw await Exception();
	}
}

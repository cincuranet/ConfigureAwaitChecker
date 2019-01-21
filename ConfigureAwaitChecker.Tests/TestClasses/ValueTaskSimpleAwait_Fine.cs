using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(CheckerProblem.NoProblem)]
public class ValueTaskSimpleAwait_Fine : TestClassBase
{
	public async ValueTask FooBar()
	{
		await ValueTask().ConfigureAwait(false);
	}
}

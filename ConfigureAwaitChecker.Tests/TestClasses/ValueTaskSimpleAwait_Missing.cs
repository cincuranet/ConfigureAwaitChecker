using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(CheckerProblem.MissingConfigureAwaitFalse)]
public class ValueTaskSimpleAwait_Missing : TestClassBase
{
	public async ValueTask FooBar()
	{
		await ValueTask();
	}
}

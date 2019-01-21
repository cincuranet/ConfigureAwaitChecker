using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(CheckerProblem.ConfigureAwaitWithTrue)]
public class ValueTaskSimpleAwait_WithTrue : TestClassBase
{
	public async ValueTask FooBar()
	{
		await ValueTask().ConfigureAwait(true);
	}
}

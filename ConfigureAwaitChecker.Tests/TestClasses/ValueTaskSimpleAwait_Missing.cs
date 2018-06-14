using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(new[] { true })]
public class ValueTaskSimpleAwait_Missing : TestClassBase
{
	public async ValueTask FooBar()
	{
		await ValueTask();
	}
}

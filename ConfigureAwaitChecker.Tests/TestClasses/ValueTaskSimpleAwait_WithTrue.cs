using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(new[] { false }, new[] { true })]
public class ValueTaskSimpleAwait_WithTrue : TestClassBase
{
	public async ValueTask FooBar()
	{
		await ValueTask().ConfigureAwait(true);
	}
}

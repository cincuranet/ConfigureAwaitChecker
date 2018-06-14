using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(new[] { false })]
public class ValueTaskSimpleAwait_Fine : TestClassBase
{
	public async ValueTask FooBar()
	{
		await ValueTask().ConfigureAwait(false);
	}
}

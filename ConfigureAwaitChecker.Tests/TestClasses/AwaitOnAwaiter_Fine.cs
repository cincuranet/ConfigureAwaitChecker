using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(CheckerProblem.NoProblem)]
public class AwaitOnAwaiter_Fine : TestClassBase
{
	public async Task FooBar()
	{
		var awaiter = F(6);
		await awaiter.ConfigureAwait(false);
	}
}
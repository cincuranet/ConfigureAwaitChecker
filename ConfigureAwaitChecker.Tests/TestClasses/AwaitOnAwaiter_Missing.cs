using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;
using ConfigureAwaitChecker.Tests.TestClasses;

[CheckerTests.ExpectedResult(CheckerProblem.MissingConfigureAwaitFalse)]
public class AwaitOnAwaiter_Missing : TestClassBase
{
	public async Task FooBar()
	{
		var awaiter = F(6);
		await awaiter;
	}
}
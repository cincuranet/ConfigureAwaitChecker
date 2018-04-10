using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests.TestClasses;

public class AwaitTaskYield : TestClassBase
{
    public async Task FooBar()
    {
        await Task.Yield();
    }
}
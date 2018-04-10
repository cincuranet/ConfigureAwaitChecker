using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ConfigureAwaitChecker.Tests.TestClasses;

public class AwaitTaskYieldAsField: TestClassBase
{
    private YieldAwaitable _yieldAwaitable = Task.Yield();

    public async Task FooBar()
    {
        await _yieldAwaitable;
    }
}
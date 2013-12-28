using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigureAwaitChecker.Tests.TestClasses
{
    public class ExecutingAsyncLambda_Fine : TestClassBase
    {
        public async Task FooBar()
        {
            await ((Func<Task>)(async () => await Task.Delay(1).ConfigureAwait(false)))().ConfigureAwait(false);
        }
    }
}

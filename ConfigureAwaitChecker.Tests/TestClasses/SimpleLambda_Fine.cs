using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigureAwaitChecker.Tests.TestClasses
{
    public class SimpleLambda_Fine : TestClassBase
    {
#pragma warning disable 1998
        public async Task FooBar()
#pragma warning restore 1998
        {
            var func = (Func<Task>)(async () => await Task.Delay(1).ConfigureAwait(false));
        }
    }
}

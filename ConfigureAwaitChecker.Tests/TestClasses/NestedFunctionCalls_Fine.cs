using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigureAwaitChecker.Tests.TestClasses
{
    public class NestedFunctionCalls_Fine : TestClassBase
    {
        public async Task FooBar()
        {
            await F(await Bool().ConfigureAwait(false)).ConfigureAwait(false);
        }
    }
}

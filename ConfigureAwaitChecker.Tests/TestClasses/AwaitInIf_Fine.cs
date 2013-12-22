using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigureAwaitChecker.Tests.TestClasses
{
    public class AwaitInIf_Fine : TestClassBase
    {
        public async Task FooBar()
        {
            if (await Bool().ConfigureAwait(false)) { }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigureAwaitChecker.Tests.TestClasses
{
    public class SimpleAwaitWithBraces_Missing : TestClassBase
    {
        public async Task FooBar()
        {
            await (Task.Delay(1));
        }
    }
}

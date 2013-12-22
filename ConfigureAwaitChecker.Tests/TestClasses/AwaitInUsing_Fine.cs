using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigureAwaitChecker.Tests.TestClasses
{
    public class AwaitInUsing_Fine : TestClassBase
    {
        public async Task FooBar()
        {
            using (var disposable = await Disposable().ConfigureAwait(false)) { }
        }
    }
}

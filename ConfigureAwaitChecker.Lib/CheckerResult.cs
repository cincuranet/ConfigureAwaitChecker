using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigureAwaitChecker.Lib
{
    public sealed class CheckerResult
    {
        public bool HasConfigureAwaitFalse { get; private set; }
        public int Line { get; private set; }
        public int Column { get; private set; }

        public CheckerResult(bool hasConfigureAwaitFalse, int line, int column)
        {
            HasConfigureAwaitFalse = hasConfigureAwaitFalse;
            Line = line;
            Column = column;
        }
    }
}

using System;
using System.IO;
using System.Reflection;

namespace ConfigureAwaitChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            var testFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Code.cs");
            var checker = new Checker(testFile);
            Console.WriteLine(checker.DebugListTree());

            foreach (var item in checker.Check())
            {
                if (!item.HasConfigureAwaitFalse)
                {
                    Console.WriteLine("ERROR: Missing 'ConfigureAwait(false)' for await on line {0} column {1}.", item.Line, item.Column);
                }
                else
                {
                    Console.WriteLine("Good. Found 'ConfigureAwait(false)' for await on line {0} column {1}.", item.Line, item.Column);
                }
            }
        }
    }
}

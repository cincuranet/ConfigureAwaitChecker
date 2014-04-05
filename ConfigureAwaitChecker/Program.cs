using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ConfigureAwaitChecker.Lib;

namespace ConfigureAwaitChecker
{
	class Program
	{
		static int Main(string[] args)
		{
			if (args.Count() != 1)
				return 1;
			if (string.IsNullOrWhiteSpace(args[0]))
				return 2;
			if (!File.Exists(args[0]))
				return 3;

			var checker = new Checker(args[0]);

			foreach (var item in checker.Check())
			{
				if (!item.HasConfigureAwaitFalse)
				{
					Console.Error.WriteLine("ERROR: Missing 'ConfigureAwait(false)' for await on line {0} column {1}.", item.Line, item.Column);
				}
				else
				{
					Console.Out.WriteLine("Good. Found 'ConfigureAwait(false)' for await on line {0} column {1}.", item.Line, item.Column);
				}
			}

			return 0;
		}
	}
}

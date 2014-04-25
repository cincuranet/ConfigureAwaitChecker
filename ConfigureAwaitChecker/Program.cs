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
				return ExitCodes.TooFewArguments;
			if (string.IsNullOrWhiteSpace(args[0]))
				return ExitCodes.ArgumentEmpty;
			if (!File.Exists(args[0]))
				return ExitCodes.FileNotFound;

			var result = ExitCodes.OK;
			var checker = new Checker(args[0]);
			foreach (var item in checker.Check())
			{
				if (!item.HasConfigureAwaitFalse)
				{
					Console.WriteLine("ERROR: Missing 'ConfigureAwait(false)' for await on line {0} column {1}.", item.Line, item.Column);
					result = ExitCodes.Error;
				}
				else
				{
					Console.WriteLine("Good. Found 'ConfigureAwait(false)' for await on line {0} column {1}.", item.Line, item.Column);
				}
			}

			return result;
		}
	}

	static class ExitCodes
	{
		public const int OK = 0;
		public const int Error = 1;
		public const int TooFewArguments = 101;
		public const int ArgumentEmpty = 102;
		public const int FileNotFound = 103;
	}
}

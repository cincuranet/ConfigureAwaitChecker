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
					ConsoleWriteLine("ERROR: Missing 'ConfigureAwait(false)' for await on line {0} column {1}.", ConsoleColor.Red, item.Line, item.Column);
					result = ExitCodes.Error;
				}
				else
				{
					ConsoleWriteLine("Good. Found 'ConfigureAwait(false)' for await on line {0} column {1}.", ConsoleColor.Green, item.Line, item.Column);
				}
			}

			return result;
		}

		static void ConsoleWriteLine(string format, ConsoleColor foregroundColor, params object[] args)
		{
			var oldForegroundColor = Console.ForegroundColor;
			Console.ForegroundColor = foregroundColor;
			Console.WriteLine(format, args);
			Console.ForegroundColor = oldForegroundColor;
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

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ConfigureAwaitChecker.Lib;

namespace ConfigureAwaitChecker.Console
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
			foreach (var item in CreateChecker(args[0]).Check())
			{
				var location = item.Location.GetMappedLineSpan().StartLinePosition;
				if (!item.HasConfigureAwaitFalse)
				{
					ConsoleWriteLine("ERROR: Missing 'ConfigureAwait(false)' for await on line {0} column {1}.", ConsoleColor.Red, location.Line, location.Character);
					result = ExitCodes.Error;
				}
				else
				{
					ConsoleWriteLine("Good. Found 'ConfigureAwait(false)' for await on line {0} column {1}.", ConsoleColor.Green, location.Line, location.Character);
				}
			}

			return result;
		}

		static Checker CreateChecker(string file)
		{
			using (var fileStream = File.OpenRead(file))
			{
				return new Checker(fileStream);
			}
		}

		static void ConsoleWriteLine(string format, ConsoleColor foregroundColor, params object[] args)
		{
			var oldForegroundColor = System.Console.ForegroundColor;
			System.Console.ForegroundColor = foregroundColor;
			System.Console.WriteLine(format, args);
			System.Console.ForegroundColor = oldForegroundColor;
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

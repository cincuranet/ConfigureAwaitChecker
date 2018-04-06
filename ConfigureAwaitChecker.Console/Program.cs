using ConfigureAwaitChecker.Analyzer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConfigureAwaitChecker.Console
{
	class Program
	{
		static int Main(string[] args)
		{
			if (args.Count() <= 1)
				return ExitCodes.TooFewArguments;
			if (string.IsNullOrWhiteSpace(args[0]))
				return ExitCodes.ArgumentEmpty;
			if (!File.Exists(args[0]))
				return ExitCodes.FileNotFound;

			var mscorlib = typeof(object).Assembly.Location;

			var result = ExitCodes.OK;
			foreach (var item in CreateChecker(args[0], new [] {mscorlib}).Check())
			{
				var location = item.Location.GetMappedLineSpan().StartLinePosition;
				if (!item.HasConfigureAwait)
				{
					ConsoleWriteLine($"ERROR: Missing `ConfigureAwait(false)` for await on line {location.Line} column {location.Character}.", ConsoleColor.Red);
					result = ExitCodes.Error;
				}
				else
				{
					ConsoleWriteLine($"GOOD: Found `ConfigureAwait(false)` for await on line {location.Line} column {location.Character}.", ConsoleColor.Green);
				}
			}

			return result;
		}

		static Checker CreateChecker(string file, IReadOnlyList<string> referenceLocations)
		{
			using (var fileStream = File.OpenRead(file))
			{
				return new Checker(fileStream, referenceLocations);
			}
		}

		static void ConsoleWriteLine(string message, ConsoleColor foregroundColor)
		{
			var oldForegroundColor = System.Console.ForegroundColor;
			System.Console.ForegroundColor = foregroundColor;
			System.Console.WriteLine(message);
			System.Console.ForegroundColor = oldForegroundColor;
		}
	}
}

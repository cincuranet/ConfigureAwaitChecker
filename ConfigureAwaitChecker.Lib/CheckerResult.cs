using Microsoft.CodeAnalysis;

namespace ConfigureAwaitChecker.Lib
{
	public sealed class CheckerResult
	{
		public CheckerProblem Problem { get; }
		public Location Location { get; }

		public CheckerResult(CheckerProblem problem, Location location)
		{
			Problem = problem;
			Location = location;
		}

		public bool NeedsFix => Problem != CheckerProblem.NoProblem;
	}
}

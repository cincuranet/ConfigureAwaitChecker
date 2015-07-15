using Microsoft.CodeAnalysis;

namespace ConfigureAwaitChecker.Analyzer
{
	public sealed class CheckerResult
    {
        public bool HasConfigureAwaitFalse { get; }
        public Location Location { get; }

        public CheckerResult(bool hasConfigureAwaitFalse, Location location)
        {
            HasConfigureAwaitFalse = hasConfigureAwaitFalse;
			Location = location;
        }
    }
}

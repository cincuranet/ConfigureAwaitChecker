using Microsoft.CodeAnalysis;

namespace ConfigureAwaitChecker.Analyzer
{
	public sealed class CheckerResult
    {
        public bool HasConfigureAwait { get; }
        public Location Location { get; }

        public CheckerResult(bool hasConfigureAwait, Location location)
        {
            HasConfigureAwait = hasConfigureAwait;
			Location = location;
        }
    }
}

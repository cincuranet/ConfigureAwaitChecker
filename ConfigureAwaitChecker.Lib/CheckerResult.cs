using Microsoft.CodeAnalysis;

namespace ConfigureAwaitChecker.Lib
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

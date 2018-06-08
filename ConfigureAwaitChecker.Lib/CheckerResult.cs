using Microsoft.CodeAnalysis;

namespace ConfigureAwaitChecker.Lib
{
	public sealed class CheckerResult
	{
		public bool NeedsConfigureAwaitFalse { get; }
		public Location Location { get; }

		public CheckerResult(bool needsConfigureAwaitFalse, Location location)
		{
			NeedsConfigureAwaitFalse = needsConfigureAwaitFalse;
			Location = location;
		}
	}
}

using Microsoft.CodeAnalysis;

namespace ConfigureAwaitChecker.Lib
{
	public sealed class CheckerResult
	{
		public bool NeedsAddConfigureAwaitFalse { get; internal set; }
		public bool NeedsSwitchConfigureAwaitToFalse { get; internal set; }
		public Location Location { get; internal set; }

		public CheckerResult(Location location)
		{
			Location = location;
		}
	}
}

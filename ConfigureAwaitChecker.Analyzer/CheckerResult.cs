using Microsoft.CodeAnalysis;

namespace ConfigureAwaitChecker.Analyzer
{
    public sealed class CheckerResult
    {
        public bool HasConfigureAwaitExpression { get; }
        public Location Location { get; }

        public CheckerResult(bool hasConfigureAwaitExpression, Location location)
        {
            HasConfigureAwaitExpression = hasConfigureAwaitExpression;
            Location = location;
        }
    }
}

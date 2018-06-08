using System.Reflection;
using NUnitLite;

namespace ConfigureAwaitChecker.Tests
{
	class Program
	{
		static int Main(string[] args)
		{
			return new AutoRun(Assembly.GetEntryAssembly()).Execute(new[] { "--noresult", "--labels=All" });
		}
	}
}

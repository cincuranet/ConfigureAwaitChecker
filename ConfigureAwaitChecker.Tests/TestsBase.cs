using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using Microsoft.CodeAnalysis;

namespace ConfigureAwaitChecker.Tests
{
	public abstract class TestsBase
	{
		protected MetadataReference[] MetadataReferences { get; } = new[]
		{
			MetadataReference.CreateFromFile(typeof(TestsBase).Assembly.Location),
			MetadataReference.CreateFromFile(typeof(Checker).Assembly.Location),
			MetadataReference.CreateFromFile(typeof(Task).Assembly.Location),
			MetadataReference.CreateFromFile(typeof(ValueTask).Assembly.Location),
			MetadataReference.CreateFromFile(typeof(IAsyncEnumerable<>).Assembly.Location),
			MetadataReference.CreateFromFile(typeof(IAsyncDisposable).Assembly.Location),
			// to force System.Runtime
			MetadataReference.CreateFromFile(typeof(WaitHandleExtensions).Assembly.Location),
		};

		public static Task<T> F<T>(T value)
		{
			return Task.FromResult(value);
		}

		public static Task<bool> Bool()
		{
			return F(true);
		}

		public static Task<IDisposable> Disposable()
		{
			return F(default(IDisposable));
		}

		public static Task<IEnumerable<object>> Enumerable()
		{
			return F(default(IEnumerable<object>));
		}

		public static Task<Exception> Exception()
		{
			return F(new Exception());
		}

		public static ValueTask ValueTask()
		{
			return new ValueTask();
		}

		public static IAsyncEnumerable<int> AsyncEnumerable()
		{
			return default;
		}

		public static IAsyncDisposable AsyncDisposable()
		{
			return default;
		}
	}
}

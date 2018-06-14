using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConfigureAwaitChecker.Tests.TestClasses
{
	public abstract class TestClassBase
    {
        protected Task<T> F<T>(T value)
        {
            return Task.FromResult(value);
        }

        protected Task<bool> Bool()
        {
            return F(true);
        }

        protected Task<IDisposable> Disposable()
        {
            return F(default(IDisposable));
        }

        protected Task<IEnumerable<object>> Enumerable()
        {
            return F(default(IEnumerable<object>));
		}

		protected Task<Exception> Exception()
		{
			return F(new Exception());
		}

		protected ValueTask ValueTask()
		{
			return new ValueTask();
		}
	}
}

using System;
using System.Data.Entity;

namespace Technical
{
    public class BaseFactory<T> : IDisposable where T : DbContext
    {

        /// <summary>
        /// Context access.
        /// </summary>
        public T Context { get; }

        /// <summary>
        /// Initialize context.
        /// </summary>
        public BaseFactory()
        {
            Context = Activator.CreateInstance<T>();
        }

        /// <summary>
        /// Dispose context.
        /// </summary>
        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}

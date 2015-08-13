using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Technical
{
    /// <summary>
    /// Linq helpers.
    /// </summary>
    public static class LinqHelper
    {

        /// <summary>
        /// Get first or default.
        /// </summary>
        /// <typeparam name="T">List type.</typeparam>
        /// <param name="query">Query.</param>
        /// <returns>List result.</returns>
        public static T FirstOrDefaultEx<T>(this IQueryable<T> query)
        {
            T result;
            try
            {
                result = query.FirstOrDefault();
            }
            catch
            {
                result = default(T);
            }
            return result;
        }

        /// <summary>
        /// Save changes.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <returns>Process result.</returns>
        public static bool SaveChangesEx(this DbContext context)
        {
            bool result;
            try
            {
                context.SaveChangesEx();
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Save changes asynchronously.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <returns>Process result.</returns>
        public static bool SaveChangesAsyncEx(this DbContext context)
        {
            bool result;
            try
            {
                context.SaveChangesAsync();
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Get list.
        /// </summary>
        /// <typeparam name="T">List type.</typeparam>
        /// <param name="query">Query.</param>
        /// <returns>List result.</returns>
        public static List<T> ToListEx<T>(this IQueryable<T> query)
        {
            List<T> result;
            try
            {
                result = query.ToList();
            }
            catch (Exception e)
            {
                result = new List<T>();
            }
            return result;
        }

    }
}

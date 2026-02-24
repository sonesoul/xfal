using System;
using System.Collections.Generic;
using System.Linq;

namespace xfal.Extensions
{
    public static class EnumerableExtensions
    {
        private readonly static Random random = new();
        public static T RandomElement<T>(this IEnumerable<T> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values), "Collection is null.");
            }

            if (!values.Any())
                throw new Exception("There are no items that can be obtained randomly.");

            return values.ElementAt(random.Next(values.Count()));
        }
    }
}
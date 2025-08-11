using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CFAN.SchoolMap.Helpers
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Safe<T>(this IEnumerable<T> col)
        {
            return col ?? new T[]{};
        }

        public static IEnumerable<TResult> SafeSelectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
        {
            if (source == null) return new TResult[]{};
            if (selector == null) return new TResult[] { };
            return source.SelectMany(s=>selector(s)?? new TResult[] { });
        }

        public static Stream ToStream(this byte[] array)
        {
            return new MemoryStream(array);
        }
    }
}

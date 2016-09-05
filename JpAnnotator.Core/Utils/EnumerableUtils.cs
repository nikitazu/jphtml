using System;
using System.Collections.Generic;
using System.Linq;

namespace JpAnnotator.Utils
{
    public static class EnumerableUtils
    {
        public static IEnumerable<T> DistinctBy<T, R>(this IEnumerable<T> items, Func<T, R> distinctionSelector)
        {
            return items.Distinct(new GenericComparer<T, R>(distinctionSelector));
        }
    }

    public class GenericComparer<T, R> : IEqualityComparer<T>
    {
        Func<T, R> _getter;

        public GenericComparer(Func<T, R> getter)
        {
            _getter = getter;
        }

        bool IEqualityComparer<T>.Equals(T x, T y)
        {
            return _getter(x).Equals(_getter(y));
        }

        int IEqualityComparer<T>.GetHashCode(T obj)
        {
            return _getter(obj).GetHashCode();
        }
    }
}

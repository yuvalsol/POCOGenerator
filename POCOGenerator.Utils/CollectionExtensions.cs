using System;
using System.Linq;

namespace System.Collections.Generic
{
    public static partial class CollectionExtensions
    {
        #region Visual Studio Search and Replace

        // if \((?<lst>[a-zA-Z0-9._]+) == null \|\| \k<lst>\.Count(?:\(\))? == 0\)
        // if (${lst}.IsNullOrEmpty())

        // if \((?<lst>[a-zA-Z0-9._]+) != null && \k<lst>\.Count(?:\(\))? > 0\)
        // if (${lst}.HasAny())

        // if \((?<lst>[a-zA-Z0-9._]+) != null && \k<lst>\.Count(?:\(\))? == 1\)
        // if (${lst}.HasSingle())

        #endregion

        public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> source)
        {
            return source == null || !source.Any();
        }

        public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return source == null || !source.Any(predicate);
        }

        public static bool IsNullOrEmpty<TSource>(this ICollection<TSource> source)
        {
            return source == null || source.Count == 0;
        }

        public static bool HasAny<TSource>(this IEnumerable<TSource> source)
        {
            return source != null && source.Any();
        }

        public static bool HasAny<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return source != null && source.Any(predicate);
        }

        public static bool HasAny<TSource>(this ICollection<TSource> source)
        {
            return source != null && source.Count > 0;
        }

        public static bool HasSingle<TSource>(this IEnumerable<TSource> source)
        {
            return source != null && source.Any() && !source.Skip(1).Any();
        }

        public static bool HasSingle<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                return false;

            bool isFoundFirst = false;

            foreach (TSource item in source)
            {
                if (predicate(item))
                {
                    if (isFoundFirst)
                        return false;
                    else
                        isFoundFirst = true;
                }
            }

            return isFoundFirst;
        }

        public static bool HasSingle<TSource>(this ICollection<TSource> source)
        {
            return source != null && source.Count == 1;
        }
    }
}
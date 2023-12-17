namespace System.Collections.Generic
{
    internal sealed class CachedEnumerable<TSource, TResult> : IEnumerable<TResult>
    {
        private readonly IEnumerable<TSource> source;
        private readonly Func<TSource, TResult> selector;
        private readonly object syncCache = new object();
        private Dictionary<TSource, TResult> cache;

        public CachedEnumerable(IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            this.source = source;
            this.selector = selector;
        }

        #region IEnumerable<TResult> Members

        public IEnumerator<TResult> GetEnumerator()
        {
            if (source == null)
                yield break;

            lock (syncCache)
            {
                if (cache == null)
                    cache = new Dictionary<TSource, TResult>();
            }

            foreach (var key in source)
            {
                TResult item;
                lock (cache)
                {
                    if (cache.TryGetValue(key, out item) == false)
                        cache.Add(key, item = selector(key));
                }
                yield return item;
            }
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}

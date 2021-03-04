using System;
using System.Collections.Generic;

namespace POCOGenerator
{
    internal sealed class CachedEnumerable<TSource, TResult> : IEnumerable<TResult>
    {
        private IEnumerable<TSource> source;
        private Func<TSource, TResult> handler;
        private object syncCache;
        private Dictionary<TSource, TResult> cache;

        public CachedEnumerable(IEnumerable<TSource> source, Func<TSource, TResult> handler)
        {
            this.source = source;
            this.handler = handler;
            this.syncCache = new object();
        }

        #region IEnumerable<TResult> Members

        public IEnumerator<TResult> GetEnumerator()
        {
            if (this.source == null)
                yield break;

            lock (this.syncCache)
            {
                if (this.cache == null)
                    this.cache = new Dictionary<TSource, TResult>();
            }

            foreach (var key in this.source)
            {
                TResult item;
                lock (this.cache)
                {
                    if (this.cache.TryGetValue(key, out item) == false)
                        this.cache.Add(key, item = this.handler(key));
                }
                yield return item;
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}

using System;
using System.Collections;
using System.Collections.Generic;

namespace Finbourne.MemoryCache
{
    public class MemoryCache : IMemoryCache
    {
        static MemoryCache instance;

        protected MemoryCache(int maximumCacheSize)
        {
        }

        public static MemoryCache Instance(int maximumCacheSize)
        {
            if (instance == null)
            {
                instance = new MemoryCache(maximumCacheSize);
            }

            return instance;
        }

        public void Add(string key, object value)
        {
            throw new NotImplementedException();
        }

        public void AddOrReplace(string key, object value)
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            throw new NotImplementedException();
        }

        public bool Contains(string key)
        {
            throw new NotImplementedException();
        }

        public object Get(string key)
        {
            throw new NotImplementedException();
        }

        public int GetCount()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}

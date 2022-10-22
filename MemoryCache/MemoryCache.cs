using System;
using System.Collections.Generic;

namespace Finbourne.MemoryCache
{
    public class MemoryCache : IMemoryCache
    {
        public MemoryCache(int maximumCacheSize)
        {
        }


        public void Add(string key, object value)
        {
            throw new NotImplementedException();
        }

        public void AddOrReplace(string key, object value)
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

        public KeyValuePair<string, object> GetAll()
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
    }
}

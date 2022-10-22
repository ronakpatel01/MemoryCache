using System;
using System.Collections.Generic;

namespace Finbourne.MemoryCache
{
    public interface IMemoryCache : IEnumerable<KeyValuePair<string, object>>
    {
        object Get(string key);
        void Add(string key, object value);
        void AddOrReplace(string key, object value);
        void Remove(string key);
        // We are assuming that the Contains Method is not used when removing the least recently used.
        bool Contains(string key);
        int GetCount();

    }
}

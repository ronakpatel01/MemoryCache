using System;
using System.Collections.Generic;

namespace MemoryCache
{
    public interface IMemoryCache
    {
        object Get(string key);
        void Add(string key, object value);
        void AddOrReplace(string key, object value);
        KeyValuePair<string, object> GetAll();
        IEnumerator<KeyValuePair<string, object>> GetEnumerator();
        bool Contains(string key);
        int GetCount();

    }
}

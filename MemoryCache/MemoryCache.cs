using System;
using System.Collections;
using System.Collections.Generic;

namespace Finbourne.MemoryCache
{
    public class MemoryCache : IMemoryCache
    {
        private class CachedObject
        {
            public int ID { get; set; }
            public object Value { get; set; }
            public DateTime UsedDateTime { get; set; }

            public CachedObject(int id, object value, DateTime usedDateTime)
            {
                ID = id;
                Value = value;
                UsedDateTime = usedDateTime;
            }
        }

        private int maximumCacheSize;
        private Dictionary<string, CachedObject> cachedObjects = new Dictionary<string, CachedObject>();
        private SortedList<int, string> cachedObjectsKeysList = new SortedList<int, string>();

        private int MaxId 
        {
            get 
            {
                if (cachedObjectsKeysList.Count == 0)
                    return -1;
                else
                    return cachedObjectsKeysList.Keys[cachedObjectsKeysList.Count - 1];
            } 
        }

        private static readonly MemoryCache instance = new MemoryCache();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="maximumCacheSize"></param>
        private MemoryCache()
        {
        }

        static MemoryCache()
        {
        }



        /// <summary>
        /// Singleton Instance
        /// </summary>
        /// <param name="maximumCacheSize"></param>
        /// <returns></returns>
        public static MemoryCache Instance
        {
            get
            {
                return instance;
            }
        }

        private void UpdateCache(string key, object value)
        {
            if (!cachedObjects.ContainsKey(key))
                return;

            int newId = MaxId + 1;

            cachedObjectsKeysList.Remove(cachedObjects[key].ID);
            cachedObjectsKeysList[newId] = key;
            cachedObjects[key] = new CachedObject(newId, value, DateTime.Now);

            CheckOverFlow();
        }

        private void AddToCache(string key, object value)
        {
            if (cachedObjects.ContainsKey(key))
                UpdateCache(key, value);
            else
            {
                int newId = MaxId + 1;
                cachedObjectsKeysList[newId] = key;
                cachedObjects[key] = new CachedObject(newId, value, DateTime.Now);

                CheckOverFlow();
            }
        }

        private void CheckOverFlow()
        {
            while (cachedObjects.Count > maximumCacheSize)
            {
                cachedObjects.Remove(cachedObjectsKeysList.Values[0]);
                cachedObjectsKeysList.Remove(cachedObjectsKeysList.Keys[0]);
            }
        }

        public void Clear()
        {
            cachedObjects = new Dictionary<string, CachedObject>();
            cachedObjectsKeysList = new SortedList<int, string>();
        }

        public void UpdateCacheSize(int cacheSize)
        {
            this.maximumCacheSize = cacheSize;
        }

        public void Add(string key, object value)
        {
            if (!cachedObjects.ContainsKey(key))
            {
                AddToCache(key, value);
            }
        }

        public void AddOrReplace(string key, object value)
        {
            AddToCache(key, value);
        }

        public void Remove(string key)
        {
            cachedObjectsKeysList.Remove(cachedObjects[key].ID);
            cachedObjects.Remove(key);
        }

        public bool Contains(string key)
        {
            return cachedObjects.ContainsKey(key);
        }

        public object Get(string key)
        {
            if (!cachedObjects.ContainsKey(key))
                return null;

            int newId = MaxId + 1;
            cachedObjectsKeysList.Remove(cachedObjects[key].ID);
            cachedObjectsKeysList[newId] = key;
            return cachedObjects[key].Value;
            
        }

        public int GetCount()
        {
            return cachedObjects.Count;
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            foreach (KeyValuePair<string, CachedObject> cachedObject in cachedObjects)
            {
                yield return new KeyValuePair<string, object>(cachedObject.Key, cachedObject.Value.Value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

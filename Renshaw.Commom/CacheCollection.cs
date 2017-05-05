using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renshaw.Commom
{
    public class CacheCollection : IDictionary<string, object>
    {
        private ConcurrentDictionary<string, object> cacheStruct;
        public CacheCollection(int capacity = 31, bool isReadOnly = false)
        {
            IsReadOnly = isReadOnly;
            capacity = capacity > 0 ? capacity : 31;
            cacheStruct = new ConcurrentDictionary<string, object>(Environment.ProcessorCount * 2, capacity);
        }
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return cacheStruct.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<string, object> item)
        {
            Add(item.Key, item.Value);
        }

        public void Add(string key, object value)
        {
            cacheStruct.TryAdd(key, value);
        }

        public void Clear()
        {
            cacheStruct.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return cacheStruct.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return cacheStruct.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            array = cacheStruct.ToArray();
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            object value;
            return cacheStruct.TryRemove(item.Key,out value);
        }

        public bool Remove(string key)
        {
            object value;
            return cacheStruct.TryRemove(key, out value);
        }

        public bool TryGetValue(string key, out object value)
        {
            return cacheStruct.TryGetValue(key, out value);
        }

        public object this[string key]
        {
            get
            {
                return cacheStruct[key];
            }
            set
            {
                cacheStruct[key] = value;
            }
        }

        public int Count => cacheStruct.Count;

        public ICollection<string> Keys => cacheStruct.Keys;

        public ICollection<object> Values => cacheStruct.Values;

        public bool IsReadOnly { get; }
    }
}

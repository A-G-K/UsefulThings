using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UsefulThings.Collections
{
    [Serializable]
    public class DictionaryContainer<TKey, TValue> : DictionaryContainer<Dictionary<TKey, TValue>, TKey, TValue> {}

    [Serializable]
    public class DictionaryContainer<T, TKey, TValue> : ISerializationCallbackReceiver, IDictionary<TKey, TValue>,
        IDictionary
        where T : IDictionary<TKey, TValue>
    {
        [SerializeField] private List<TKey> keys = new List<TKey>();
        [SerializeField] private List<TValue> values = new List<TValue>();

        public readonly T inside;

        public int Count => inside.Count;
        public bool IsSynchronized => InsideIDictionary.IsSynchronized;
        public object SyncRoot => InsideIDictionary.SyncRoot;
        public bool IsReadOnly => inside.IsReadOnly;
        public ICollection<TKey> Keys => inside.Keys;
        ICollection IDictionary.Values => InsideIDictionary.Keys;
        ICollection IDictionary.Keys => InsideIDictionary.Values;
        public ICollection<TValue> Values => inside.Values;
        public bool IsFixedSize => InsideIDictionary.IsFixedSize;
        private IDictionary InsideIDictionary => (IDictionary) inside;

        public DictionaryContainer()
        {
            inside = Activator.CreateInstance<T>();
        }

        public TValue this[TKey key]
        {
            get { return inside[key]; }
            set { inside[key] = value; }
        }

        public object this[object key]
        {
            get { return InsideIDictionary[key]; }
            set { InsideIDictionary[key] = value; }
        }

        public void CopyTo(Array array, int index)
        {
            InsideIDictionary.CopyTo(array, index);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return inside.TryGetValue(key, out value);
        }

        public void Add(TKey key, TValue value)
        {
            inside.Add(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return inside.ContainsKey(key);
        }

        public bool Remove(TKey key)
        {
            return inside.Remove(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            inside.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> key)
        {
            return inside.Remove(key);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            inside.Add(item);
        }

        public void Add(object key, object value)
        {
            InsideIDictionary.Add(key, value);
        }

        public void Clear()
        {
            inside.Clear();
        }

        public bool Contains(object key)
        {
            return InsideIDictionary.Contains(key);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return ((IDictionary) inside).GetEnumerator();
        }

        public void Remove(object key)
        {
            InsideIDictionary.Remove(key);
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return inside.Contains(item);
        }

        public void OnBeforeSerialize()
        {
            // We need a decent way to sync the dictionary towards the serialized list, we can't just clear the serialized
            // list because new values might be added there (from the editor)
            HashSet<TKey> keysSet = new HashSet<TKey>(keys);

            foreach (var pair in inside)
            {
                if (!keysSet.Contains(pair.Key))
                {
                    keys.Add(pair.Key);
                    values.Add(pair.Value);
                }
            }
        }

        public void OnAfterDeserialize()
        {
            inside.Clear();

            for (int i = 0; i < keys.Count; i++)
            {
                inside[keys[i]] = values[i];
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return inside.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
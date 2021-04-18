using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UsefulThings.Collections
{
    [Serializable]
    public class HashSetContainer<TValue> : SetContainer<HashSet<TValue>, TValue> {}
    
    [Serializable]
    public class SortedSetContainer<TValue> : SetContainer<SortedSet<TValue>, TValue> {}
    
    [Serializable]
    public class SetContainer<T, TValue> : ISerializationCallbackReceiver, IEnumerable<TValue>
        where T : ISet<TValue>
    {
        [SerializeField]
        private List<TValue> setValues = new List<TValue>();
        public readonly ISet<TValue> inside;

        public int Count => inside.Count;
        
        public SetContainer()
        {
            inside = Activator.CreateInstance<T>();
        }

        public void Add(TValue item)
        {
            inside.Add(item);
        }

        public bool Contains(TValue item)
        {
            return inside.Contains(item);
        }

        public bool Remove(TValue item)
        {
            return inside.Remove(item);
        }

        public void Clear()
        {
            inside.Clear();
        }

        public void OnBeforeSerialize()
        {
            setValues.Clear();
            
            foreach (TValue item in inside)
            {
                setValues.Add(item);
            }
        }

        public void OnAfterDeserialize()
        {
            inside.Clear();
            
            foreach (TValue item in setValues)
            {
                inside.Add(item);
            }
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return inside.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

#if UNITY_EDITOR
    public class SetContainerUtils
    {
        private const string SetName = "setValues";
        
        public static SerializedProperty GetArrayProperty(SerializedProperty serializedProperty)
        {
            return serializedProperty.FindPropertyRelative(SetName);
        }
    }
#endif
}
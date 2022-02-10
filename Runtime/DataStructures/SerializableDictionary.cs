using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using WhateverDevs.Core.Runtime.Common;

namespace WhateverDevs.Core.Runtime.DataStructures
{
    /// <summary>
    /// Serializable dictionary based on a list.
    /// For sure not as efficient as a .net dictionary, but it is serializable.
    /// </summary>
    /// <typeparam name="TK">Type of key.</typeparam>
    /// <typeparam name="TV">Type of value.</typeparam>
    [Serializable]
    public class SerializableDictionary<TK, TV> : List<ObjectPair<TK, TV>>,
                                                  IDictionary<TK, TV>,
                                                  ISerializationCallbackReceiver,
                                                  IDeserializationCallback,
                                                  ISerializable
    {
        /// <summary>
        /// Backup list that will serialize.
        /// </summary>
        public List<ObjectPair<TK, TV>> SerializedList = new();
        
        /// <summary>
        /// Out dictionaries will never be read only.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Return a collection of the dictionary keys.
        /// </summary>
        public ICollection<TK> Keys
        {
            get
            {
                List<TK> keyList = new();

                for (int i = 0; i < Count; ++i) keyList.Add(this[i].Key);

                return keyList;
            }
        }

        /// <summary>
        /// Return a collection of the dictionary values.
        /// </summary>
        public ICollection<TV> Values
        {
            get
            {
                List<TV> valueList = new();

                for (int i = 0; i < Count; ++i) valueList.Add(this[i].Value);

                return valueList;
            }
        }

        /// <summary>
        /// Access a value by its key.
        /// </summary>
        /// <param name="key">The key to use.</param>
        /// <exception cref="KeyNotFoundException">Thrown when the key does not exist in the dictionary.</exception>
        public TV this[TK key]
        {
            get
            {
                for (int i = 0; i < Count; ++i)
                    if (Compare(this[i].Key, key))
                        return this[i].Value;

                throw new KeyNotFoundException();
            }
            set => Add(key, value);
        }

        /// <summary>
        /// Does the dictionary contain the given key?
        /// </summary>
        /// <param name="key">Given key.</param>
        /// <returns>True if it contains it.</returns>
        public bool ContainsKey(TK key)
        {
            for (int i = 0; i < Count; ++i)
                if (Compare(this[i].Key, key))
                    return true;

            return false;
        }

        /// <summary>
        /// Does the dictionary contain the given key and value?
        /// </summary>
        /// <param name="item">The key and value to check.</param>
        /// <returns>True if it contains them.</returns>
        public bool Contains(KeyValuePair<TK, TV> item)
        {
            for (int i = 0; i < Count; ++i)
                if (Compare(this[i].Key, item.Key) && Compare(this[i].Value, item.Value))
                    return true;

            return false;
        }

        /// <summary>
        /// Try get a value.
        /// </summary>
        /// <param name="key">Key to access.</param>
        /// <param name="value">Value that will be returned.</param>
        /// <returns>True if successful.</returns>
        public bool TryGetValue(TK key, out TV value)
        {
            value = default;

            for (int i = 0; i < Count; ++i)
                if (Compare(this[i].Key, key))
                {
                    value = this[i].Value;
                    return true;
                }

            return false;
        }

        /// <summary>
        /// Add a key and a value.
        /// </summary>
        /// <param name="key">Key to add.</param>
        /// <param name="value">Value to add.</param>
        public void Add(TK key, TV value)
        {
            for (int i = 0; i < Count; ++i)
            {
                ObjectPair<TK, TV> pair = this[i];
                if (!Compare(pair.Key, key)) continue;
                pair.Value = value;
                return;
            }

            Add(new ObjectPair<TK, TV>
                {
                    Key = key,
                    Value = value
                });
        }

        /// <summary>
        /// Add a key and a value.
        /// </summary>
        /// <param name="item">Key and value to add.</param>
        public void Add(KeyValuePair<TK, TV> item) => Add(item.Key, item.Value);

        /// <summary>
        /// Remove the given key and its value.
        /// </summary>
        /// <param name="key">The key to remove.</param>
        /// <returns>True if successful.</returns>
        public bool Remove(TK key)
        {
            ObjectPair<TK, TV> pairToRemove = null;

            for (int i = 0; i < Count; ++i)
            {
                ObjectPair<TK, TV> pair = this[i];
                if (!Compare(pair.Key, key)) continue;
                pairToRemove = pair;
            }

            return pairToRemove != null && Remove(pairToRemove);
        }

        /// <summary>
        /// Remove the given key and value.
        /// </summary>
        /// <param name="item">The key and value to remove.</param>
        /// <returns>True if successful.</returns>
        public bool Remove(KeyValuePair<TK, TV> item)
        {
            ObjectPair<TK, TV> pairToRemove = null;

            for (int i = 0; i < Count; ++i)
            {
                ObjectPair<TK, TV> pair = this[i];
                (TK key, TV value) = item;
                if (!Compare(pair.Key, key) || !Compare(pair.Value, value)) continue;
                pairToRemove = pair;
            }

            return pairToRemove != null && Remove(pairToRemove);
        }

        /// <summary>
        /// Get an enumerator of the key value pair list.
        /// </summary>
        /// <returns>The enumerator</returns>
        public new IEnumerator<KeyValuePair<TK, TV>> GetEnumerator()
        {
            List<KeyValuePair<TK, TV>> enumerableList = new();

            for (int i = 0; i < Count; ++i)
            {
                ObjectPair<TK, TV> pair = this[i];

                enumerableList.Add(new KeyValuePair<TK, TV>(pair.Key, pair.Value));
            }

            return enumerableList.GetEnumerator();
        }

        /// <summary>
        /// Copy the collection to an array.
        /// </summary>
        // ReSharper disable twice RedundantAssignment
        public void CopyTo(KeyValuePair<TK, TV>[] array, int arrayIndex)
        {
            List<KeyValuePair<TK, TV>> temporalList = new();

            for (int i = arrayIndex; i < Count; ++i)
            {
                ObjectPair<TK, TV> pair = this[i];

                temporalList.Add(new KeyValuePair<TK, TV>(pair.Key, pair.Value));
            }

            array = temporalList.ToArray();
        }

        /// <summary>
        /// Compare two objects.
        /// </summary>
        /// <param name="firstObject"></param>
        /// <param name="secondObject"></param>
        /// <returns></returns>
        private bool Compare<T>(T firstObject, T secondObject)
        {
            // ReSharper disable twice CompareNonConstrainedGenericWithNull
            if (firstObject == null || secondObject == null) return false;

            EqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;

            return equalityComparer.Equals(firstObject, secondObject);
        }

        /// <summary>
        /// Save values to the backup list.
        /// </summary>
        public void OnBeforeSerialize()
        {
            SerializedList ??= new List<ObjectPair<TK, TV>>();
            SerializedList.Clear();

            foreach ((TK key, TV value) in this)
                SerializedList.Add(new ObjectPair<TK, TV>
                                   {
                                       Key = key,
                                       Value = value
                                   });
        }

        /// <summary>
        /// Load values from the backup list.
        /// </summary>
        public void OnAfterDeserialize()
        {
            Clear();
            foreach (ObjectPair<TK, TV> objectPair in SerializedList) Add(objectPair);
        }

        /// <summary>
        /// Deserialize the backup list.
        /// </summary>
        /// <param name="sender"></param>
        public void OnDeserialization(object sender) =>
            ((IDeserializationCallback)SerializedList).OnDeserialization(sender);

        /// <summary>
        /// Get the object data of the backup list.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context) =>
            ((ISerializable)SerializedList).GetObjectData(info, context);

        /// <summary>
        /// Shallow clones the dictionary.
        /// </summary>
        /// <returns>A shallow clone.</returns>
        public SerializableDictionary<TK, TV> ShallowClone() => new() { SerializedList = SerializedList.ShallowClone() };
    }
}
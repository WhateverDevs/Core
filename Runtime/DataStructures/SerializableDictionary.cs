using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

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
        public SerializableDictionary<TK, TV> ShallowClone()
        {
            SerializableDictionary<TK, TV> clone = new();

            foreach ((TK key, TV value) in this) clone[key] = value;

            return clone;
        }
        
        

        // Redirecting these methods is necessary since this class implements two different IEnumerable<> so generic inference doesn't work.
        // Warning, these were generated by Chat GPT, I hope it didn't miss any.

        #region Linq Support

        /// <summary>Filters a sequence of values based on a predicate.</summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains elements from the input sequence that satisfy the condition.</returns>
        public IEnumerable<ObjectPair<TK, TV>> Where(Func<ObjectPair<TK, TV>, bool> predicate) =>
            SerializedList.Where(predicate);

        /// <summary>Filters a sequence of values based on a predicate. Each element's index is used in the logic of the predicate function.</summary>
        /// <param name="predicate">A function to test each source element for a condition; the second parameter of the function represents the index of the source element.</param>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains elements from the input sequence that satisfy the condition.</returns>
        public IEnumerable<ObjectPair<TK, TV>> Where(Func<ObjectPair<TK, TV>, int, bool> predicate) =>
            SerializedList.Where(predicate);

        /// <summary>Projects each element of a sequence into a new form.</summary>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <typeparam name="TResult">The type of the value returned by <paramref name="selector" />.</typeparam>
        public IEnumerable<TResult> Select<TResult>(Func<ObjectPair<TK, TV>, TResult> selector) =>
            SerializedList.Select(selector);

        /// <summary>Projects each element of a sequence into a new form by incorporating the element's index.</summary>
        /// <param name="selector">A transform function to apply to each source element; the second parameter of the function represents the index of the source element.</param>
        /// <typeparam name="TResult">The type of the value returned by <paramref name="selector" />.</typeparam>
        public IEnumerable<TResult> Select<TResult>(Func<ObjectPair<TK, TV>, int, TResult> selector) =>
            SerializedList.Select(selector);

        /// <summary>Projects each element of a sequence to an <see cref="T:System.Collections.Generic.IEnumerable`1" /> and flattens the resulting sequences into one sequence.</summary>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <typeparam name="TResult">The type of the elements of the sequence returned by <paramref name="selector" />.</typeparam>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose elements are the result of invoking the one-to-many transform function on each element of the input sequence.</returns>
        public IEnumerable<TResult> SelectMany<TResult>(Func<ObjectPair<TK, TV>, IEnumerable<TResult>> selector) =>
            SerializedList.SelectMany(selector);

        /// <summary>Projects each element of a sequence to an <see cref="T:System.Collections.Generic.IEnumerable`1" />, and flattens the resulting sequences into one sequence. The index of each source element is used in the projected form of that element.</summary>
        /// <param name="selector">A transform function to apply to each source element; the second parameter of the function represents the index of the source element.</param>
        /// <typeparam name="TResult">The type of the elements of the sequence returned by <paramref name="selector" />.</typeparam>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose elements are the result of invoking the one-to-many transform function on each element of an input sequence.</returns>
        public IEnumerable<TResult> SelectMany<TResult>(Func<ObjectPair<TK, TV>, int, IEnumerable<TResult>> selector) =>
            SerializedList.SelectMany(selector);

        /// <summary>Projects each element of a sequence to an <see cref="T:System.Collections.Generic.IEnumerable`1" />, flattens the resulting sequences into one sequence, and invokes a result selector function on each element therein. The index of each source element is used in the intermediate projected form of that element.</summary>
        /// <param name="collectionSelector">A transform function to apply to each source element; the second parameter of the function represents the index of the source element.</param>
        /// <param name="resultSelector">A transform function to apply to each element of the intermediate sequence.</param>
        /// <typeparam name="TCollection">The type of the intermediate elements collected by <paramref name="collectionSelector" />.</typeparam>
        /// <typeparam name="TResult">The type of the elements of the resulting sequence.</typeparam>
        public IEnumerable<TResult> SelectMany<TResult, TCollection>(
            Func<ObjectPair<TK, TV>, IEnumerable<TCollection>> collectionSelector,
            Func<ObjectPair<TK, TV>, TCollection, TResult> resultSelector) =>
            SerializedList.SelectMany(collectionSelector, resultSelector);

        /// <summary>Projects each element of a sequence to an <see cref="T:System.Collections.Generic.IEnumerable`1" />, flattens the resulting sequences into one sequence, and invokes a result selector function on each element therein. The index of each source element is used in the intermediate projected form of that element.</summary>
        /// <param name="collectionSelector">A transform function to apply to each source element; the second parameter of the function represents the index of the source element.</param>
        /// <param name="resultSelector">A transform function to apply to each element of the intermediate sequence.</param>
        /// <typeparam name="TCollection">The type of the intermediate elements collected by <paramref name="collectionSelector" />.</typeparam>
        /// <typeparam name="TResult">The type of the elements of the resulting sequence.</typeparam>
        public IEnumerable<TResult> SelectMany<TResult, TCollection>(
            Func<ObjectPair<TK, TV>, int, IEnumerable<TCollection>> collectionSelector,
            Func<ObjectPair<TK, TV>, TCollection, TResult> resultSelector) =>
            SerializedList.SelectMany(collectionSelector, resultSelector);

        /// <summary>Returns a specified number of contiguous elements from the start of a sequence.</summary>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains the specified number of elements from the start of the input sequence.</returns>
        public IEnumerable<ObjectPair<TK, TV>> Take(int offset) => SerializedList.Take(offset);

        /// <summary>Returns elements from a sequence as long as a specified condition is true.</summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains the elements from the input sequence that occur before the element at which the test no longer passes.</returns>
        public IEnumerable<ObjectPair<TK, TV>> TakeWhile(Func<ObjectPair<TK, TV>, bool> predicate) =>
            SerializedList.TakeWhile(predicate);

        /// <summary>Returns elements from a sequence as long as a specified condition is true. The element's index is used in the logic of the predicate function.</summary>
        /// <param name="predicate">A function to test each source element for a condition; the second parameter of the function represents the index of the source element.</param>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains elements from the input sequence that occur before the element at which the test no longer passes.</returns>
        public IEnumerable<ObjectPair<TK, TV>> TakeWhile(Func<ObjectPair<TK, TV>, int, bool> predicate) =>
            SerializedList.TakeWhile(predicate);

        /// <summary>Bypasses a specified number of elements in a sequence and then returns the remaining elements.</summary>
        /// <param name="count">The number of elements to skip before returning the remaining elements.</param>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains the elements that occur after the specified index in the input sequence.</returns>
        public IEnumerable<ObjectPair<TK, TV>> Skip(int count) => SerializedList.Skip(count);

        /// <summary>Bypasses elements in a sequence as long as a specified condition is true and then returns the remaining elements.</summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains the elements from the input sequence starting at the first element in the linear series that does not pass the test specified by <paramref name="predicate" />.</returns>
        public IEnumerable<ObjectPair<TK, TV>> SkipWhile(Func<ObjectPair<TK, TV>, bool> predicate) =>
            SerializedList.SkipWhile(predicate);

        /// <summary>Bypasses elements in a sequence as long as a specified condition is true and then returns the remaining elements. The element's index is used in the logic of the predicate function.</summary>
        /// <param name="predicate">A function to test each source element for a condition; the second parameter of the function represents the index of the source element.</param>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains the elements from the input sequence starting at the first element in the linear series that does not pass the test specified by <paramref name="predicate" />.</returns>
        public IEnumerable<ObjectPair<TK, TV>> SkipWhile(Func<ObjectPair<TK, TV>, int, bool> predicate) =>
            SerializedList.SkipWhile(predicate);

        /// <summary>Correlates the elements of two sequences based on matching keys. The default equality comparer is used to compare keys.</summary>
        /// <param name="inner">The sequence to join to the first sequence.</param>
        /// <param name="outerKeySelector">A function to extract the join key from each element of the first sequence.</param>
        /// <param name="innerKeySelector">A function to extract the join key from each element of the second sequence.</param>
        /// <param name="resultSelector">A function to create a result element from two matching elements.</param>
        /// <typeparam name="TResult">The type of the result elements.</typeparam>
        public IEnumerable<TResult> Join<TResult>(IEnumerable<TResult> inner,
                                                  Func<ObjectPair<TK, TV>, TResult> outerKeySelector,
                                                  Func<TResult, TResult> innerKeySelector,
                                                  Func<ObjectPair<TK, TV>, TResult, TResult> resultSelector) =>
            SerializedList.Join(inner, outerKeySelector, innerKeySelector, resultSelector);

        /// <summary>Correlates the elements of two sequences based on matching keys. A specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> is used to compare keys.</summary>
        /// <param name="inner">The sequence to join to the first sequence.</param>
        /// <param name="outerKeySelector">A function to extract the join key from each element of the first sequence.</param>
        /// <param name="innerKeySelector">A function to extract the join key from each element of the second sequence.</param>
        /// <param name="resultSelector">A function to create a result element from two matching elements.</param>
        /// <param name="comparer">An <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> to hash and compare keys.</param>
        /// <typeparam name="TResult">The type of the result elements.</typeparam>
        public IEnumerable<TResult> Join<TResult>(IEnumerable<TResult> inner,
                                                  Func<ObjectPair<TK, TV>, TResult> outerKeySelector,
                                                  Func<TResult, TResult> innerKeySelector,
                                                  Func<ObjectPair<TK, TV>, TResult, TResult> resultSelector,
                                                  IEqualityComparer<TResult> comparer) =>
            SerializedList.Join(inner, outerKeySelector, innerKeySelector, resultSelector, comparer);

        /// <summary>Concatenates two sequences.</summary>
        /// <param name="second">The sequence to concatenate to the first sequence.</param>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains the concatenated elements of the two input sequences.</returns>
        public IEnumerable<ObjectPair<TK, TV>> Concat(IEnumerable<ObjectPair<TK, TV>> second) =>
            SerializedList.Concat(second);

        /// <summary>Returns distinct elements from a sequence by using the default equality comparer to compare values.</summary>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains distinct elements from the source sequence.</returns>
        public IEnumerable<ObjectPair<TK, TV>> Distinct() => SerializedList.Distinct();

        /// <summary>Returns distinct elements from a sequence by using a specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> to compare values.</summary>
        /// <param name="comparer">An <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> to compare values.</param>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains distinct elements from the source sequence.</returns>
        public IEnumerable<ObjectPair<TK, TV>> Distinct(IEqualityComparer<ObjectPair<TK, TV>> comparer) =>
            SerializedList.Distinct(comparer);

        /// <summary>Returns the first element of a sequence.</summary>
        /// <returns>The first element in the specified sequence.</returns>
        /// <exception cref="T:System.InvalidOperationException">The source sequence is empty.</exception>
        public ObjectPair<TK, TV> First() => SerializedList.First();

        /// <summary>Returns the first element of a sequence.</summary>
        /// <returns>The first element in the specified sequence.</returns>
        /// <exception cref="T:System.InvalidOperationException">The source sequence is empty.</exception>
        public ObjectPair<TK, TV> First(Func<ObjectPair<TK, TV>, bool> predicate) => SerializedList.First(predicate);

        /// <summary>Returns the first element of a sequence, or a default value if the sequence contains no elements.</summary>
        public ObjectPair<TK, TV> FirstOrDefault() => SerializedList.FirstOrDefault();

        /// <summary>Returns the first element of the sequence that satisfies a condition or a default value if no such element is found.</summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        public ObjectPair<TK, TV> FirstOrDefault(Func<ObjectPair<TK, TV>, bool> predicate) =>
            SerializedList.FirstOrDefault(predicate);

        /// <summary>Returns the last element of a sequence.</summary>
        /// <returns>The value at the last position in the source sequence.</returns>
        /// <exception cref="T:System.InvalidOperationException">The source sequence is empty.</exception>
        public ObjectPair<TK, TV> Last() => SerializedList.Last();

        /// <summary>Returns the last element of a sequence that satisfies a specified condition.</summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>The last element in the sequence that passes the test in the specified predicate function.</returns>
        /// <exception cref="T:System.InvalidOperationException">No element satisfies the condition in <paramref name="predicate" />.-or-The source sequence is empty.</exception>
        public ObjectPair<TK, TV> Last(Func<ObjectPair<TK, TV>, bool> predicate) => SerializedList.Last(predicate);

        /// <summary>Returns the last element of a sequence, or a default value if the sequence contains no elements.</summary>
        public ObjectPair<TK, TV> LastOrDefault() => SerializedList.LastOrDefault();

        /// <summary>Returns the last element of a sequence that satisfies a condition or a default value if no such element is found.</summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        public ObjectPair<TK, TV> LastOrDefault(Func<ObjectPair<TK, TV>, bool> predicate) =>
            SerializedList.LastOrDefault(predicate);

        /// <summary>Returns the only element of a sequence, and throws an exception if there is not exactly one element in the sequence.</summary>
        /// <returns>The single element of the input sequence.</returns>
        /// <exception cref="T:System.InvalidOperationException">The input sequence contains more than one element.-or-The input sequence is empty.</exception>
        public ObjectPair<TK, TV> Single() => SerializedList.Single();

        /// <summary>Returns the only element of a sequence that satisfies a specified condition, and throws an exception if more than one such element exists.</summary>
        /// <param name="predicate">A function to test an element for a condition.</param>
        /// <returns>The single element of the input sequence that satisfies a condition.</returns>
        /// <exception cref="T:System.InvalidOperationException">No element satisfies the condition in <paramref name="predicate" />.-or-More than one element satisfies the condition in <paramref name="predicate" />.-or-The source sequence is empty.</exception>
        public ObjectPair<TK, TV> Single(Func<ObjectPair<TK, TV>, bool> predicate) => SerializedList.Single(predicate);

        /// <summary>Returns the only element of a sequence, or a default value if the sequence is empty; this method throws an exception if there is more than one element in the sequence.</summary>
        /// <exception cref="T:System.InvalidOperationException">The input sequence contains more than one element.</exception>
        public ObjectPair<TK, TV> SingleOrDefault() => SerializedList.SingleOrDefault();

        /// <summary>Returns the only element of a sequence that satisfies a specified condition or a default value if no such element exists; this method throws an exception if more than one element satisfies the condition.</summary>
        /// <param name="predicate">A function to test an element for a condition.</param>
        public ObjectPair<TK, TV> SingleOrDefault(Func<ObjectPair<TK, TV>, bool> predicate) =>
            SerializedList.SingleOrDefault(predicate);

        /// <summary>Returns the element at a specified index in a sequence.</summary>
        /// <param name="index">The zero-based index of the element to retrieve.</param>
        /// <returns>The element at the specified position in the source sequence.</returns>
        public ObjectPair<TK, TV> ElementAt(int index) => SerializedList.ElementAt(index);

        /// <summary>Returns the element at a specified index in a sequence or a default value if the index is out of range.</summary>
        /// <param name="index">The zero-based index of the element to retrieve.</param>
        public ObjectPair<TK, TV> ElementAtOrDefault(int index) => SerializedList.ElementAtOrDefault(index);

        /// <summary>Produces the set difference of two sequences by using the default equality comparer to compare values.</summary>
        /// <param name="second">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose elements that also occur in the first sequence will cause those elements to be removed from the returned sequence.</param>
        /// <returns>A sequence that contains the set difference of the elements of two sequences.</returns>
        public IEnumerable<ObjectPair<TK, TV>> Except(IEnumerable<ObjectPair<TK, TV>> second) =>
            SerializedList.Except(second);

        /// <summary>Produces the set difference of two sequences by using the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> to compare values.</summary>
        /// <param name="second">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose elements that also occur in the first sequence will cause those elements to be removed from the returned sequence.</param>
        /// <param name="comparer">An <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> to compare values.</param>
        /// <returns>A sequence that contains the set difference of the elements of two sequences.</returns>
        public IEnumerable<ObjectPair<TK, TV>> Except(IEnumerable<ObjectPair<TK, TV>> second,
                                                      IEqualityComparer<ObjectPair<TK, TV>> comparer) =>
            SerializedList.Except(second, comparer);

        /// <summary>Produces the set intersection of two sequences by using the default equality comparer to compare values.</summary>
        /// <param name="second">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose distinct elements that also appear in the first sequence will be returned.</param>
        /// <returns>A sequence that contains the elements that form the set intersection of two sequences.</returns>
        public IEnumerable<ObjectPair<TK, TV>> Intersect(IEnumerable<ObjectPair<TK, TV>> second) =>
            SerializedList.Intersect(second);

        /// <summary>Produces the set intersection of two sequences by using the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> to compare values.</summary>
        /// <param name="second">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose distinct elements that also appear in the first sequence will be returned.</param>
        /// <param name="comparer">An <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> to compare values.</param>
        /// <returns>A sequence that contains the elements that form the set intersection of two sequences.</returns>
        public IEnumerable<ObjectPair<TK, TV>> Intersect(IEnumerable<ObjectPair<TK, TV>> second,
                                                         IEqualityComparer<ObjectPair<TK, TV>> comparer) =>
            SerializedList.Intersect(second, comparer);

        /// <summary>Returns the minimum value in a generic sequence.</summary>
        /// <returns>The minimum value in the sequence.</returns>
        public ObjectPair<TK, TV> Min() => SerializedList.Min();

        /// <summary>Invokes a transform function on each element of a generic sequence and returns the minimum resulting value.</summary>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <typeparam name="TResult">The type of the value returned by <paramref name="selector" />.</typeparam>
        /// <returns>The minimum value in the sequence.</returns>
        public TResult Min<TResult>(Func<ObjectPair<TK, TV>, TResult> selector) => SerializedList.Min(selector);

        /// <summary>Returns the maximum value in a generic sequence.</summary>
        /// <returns>The maximum value in the sequence.</returns>
        public ObjectPair<TK, TV> Max() => SerializedList.Max();

        /// <summary>Invokes a transform function on each element of a generic sequence and returns the maximum resulting value.</summary>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <typeparam name="TResult">The type of the value returned by <paramref name="selector" />.</typeparam>
        /// <returns>The maximum value in the sequence.</returns>
        public TResult Max<TResult>(Func<ObjectPair<TK, TV>, TResult> selector) => SerializedList.Max(selector);

        /// <summary>Sorts the elements of a sequence in ascending order according to a key.</summary>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <returns>An <see cref="T:System.Linq.IOrderedEnumerable`1" /> whose elements are sorted according to a key.</returns>
        public IEnumerable<ObjectPair<TK, TV>> OrderBy(Func<ObjectPair<TK, TV>, IComparable> keySelector) =>
            SerializedList.OrderBy(keySelector);

        /// <summary>Sorts the elements of a sequence in descending order according to a key.</summary>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <returns>An <see cref="T:System.Linq.IOrderedEnumerable`1" /> whose elements are sorted in descending order according to a key.</returns>
        public IEnumerable<ObjectPair<TK, TV>> OrderByDescending(Func<ObjectPair<TK, TV>, IComparable> keySelector) =>
            SerializedList.OrderByDescending(keySelector);

        /// <summary>Produces the set union of two sequences by using the default equality comparer.</summary>
        /// <param name="second">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose distinct elements form the second set for the union.</param>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains the elements from both input sequences, excluding duplicates.</returns>
        public IEnumerable<ObjectPair<TK, TV>> Union(IEnumerable<ObjectPair<TK, TV>> second) =>
            SerializedList.Union(second);

        /// <summary>Produces the set union of two sequences by using a specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.</summary>
        /// <param name="second">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose distinct elements form the second set for the union.</param>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> to compare values.</param>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains the elements from both input sequences, excluding duplicates.</returns>
        public IEnumerable<ObjectPair<TK, TV>> Union(IEnumerable<ObjectPair<TK, TV>> second,
                                                     IEqualityComparer<ObjectPair<TK, TV>> comparer) =>
            SerializedList.Union(second, comparer);

        /// <summary>Groups the elements of a sequence according to a specified key selector function and projects the elements for each group by using a specified function.</summary>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="elementSelector">A function to map each source element to an element in the <see cref="T:System.Linq.IGrouping`2" />.</param>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
        /// <typeparam name="TElement">The type of the elements in the <see cref="T:System.Linq.IGrouping`2" />.</typeparam>
        /// <returns>An IEnumerable&lt;IGrouping&lt;TKey, TElement&gt;&gt; in C# or IEnumerable(Of IGrouping(Of TKey, TElement)) in Visual Basic where each <see cref="T:System.Linq.IGrouping`2" /> object contains a collection of objects of type <paramref name="TElement" /> and a key.</returns>
        public IEnumerable<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(
            Func<ObjectPair<TK, TV>, TKey> keySelector,
            Func<ObjectPair<TK, TV>, TElement>
                elementSelector) =>
            SerializedList.GroupBy(keySelector, elementSelector);

        /// <summary>Groups the elements of a sequence according to a key selector function. The keys are compared by using a comparer and each group's elements are projected by using a specified function.</summary>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="elementSelector">A function to map each source element to an element in an <see cref="T:System.Linq.IGrouping`2" />.</param>
        /// <param name="comparer">An <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> to compare keys.</param>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
        /// <typeparam name="TElement">The type of the elements in the <see cref="T:System.Linq.IGrouping`2" />.</typeparam>
        /// <returns>An IEnumerable&lt;IGrouping&lt;TKey, TElement&gt;&gt; in C# or IEnumerable(Of IGrouping(Of TKey, TElement)) in Visual Basic where each <see cref="T:System.Linq.IGrouping`2" /> object contains a collection of objects of type <paramref name="TElement" /> and a key.</returns>
        public IEnumerable<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(
            Func<ObjectPair<TK, TV>, TKey> keySelector,
            Func<ObjectPair<TK, TV>, TElement>
                elementSelector,
            IEqualityComparer<TKey> comparer) =>
            SerializedList.GroupBy(keySelector, elementSelector, comparer);

        /// <summary>Groups the elements of a sequence according to a specified key selector function and creates a result value from each group and its key.</summary>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="resultSelector">A function to create a result value from each group.</param>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
        /// <typeparam name="TResult">The type of the result value returned by <paramref name="resultSelector" />.</typeparam>
        public IEnumerable<TResult> GroupBy<TKey, TResult>(Func<ObjectPair<TK, TV>, TKey> keySelector,
                                                           Func<TKey, IEnumerable<ObjectPair<TK, TV>>,
                                                               TResult> resultSelector) =>
            SerializedList.GroupBy(keySelector, resultSelector);

        /// <summary>Groups the elements of a sequence according to a specified key selector function and creates a result value from each group and its key. The keys are compared by using a specified comparer.</summary>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="resultSelector">A function to create a result value from each group.</param>
        /// <param name="comparer">An <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> to compare keys with.</param>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
        /// <typeparam name="TResult">The type of the result value returned by <paramref name="resultSelector" />.</typeparam>
        public IEnumerable<TResult> GroupBy<TKey, TResult>(Func<ObjectPair<TK, TV>, TKey> keySelector,
                                                           Func<TKey, IEnumerable<ObjectPair<TK, TV>>,
                                                               TResult> resultSelector,
                                                           IEqualityComparer<TKey> comparer) =>
            SerializedList.GroupBy(keySelector, resultSelector, comparer);

        /// <summary>Filters the elements of an <see cref="T:System.Collections.IEnumerable" /> based on a specified type.</summary>
        /// <typeparam name="TResult">The type to filter the elements of the sequence on.</typeparam>
        public IEnumerable<TResult> OfType<TResult>() => SerializedList.OfType<TResult>();

        /// <summary>Casts the elements of an <see cref="T:System.Collections.IEnumerable" /> to the specified type.</summary>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains each element of the source sequence cast to the specified type.</returns>
        public IEnumerable<TResult> Cast<TResult>() => SerializedList.Cast<TResult>();

        /// <summary>Applies a specified function to the corresponding elements of two sequences, producing a sequence of the results.</summary>
        /// <param name="second">The second sequence to merge.</param>
        /// <param name="resultSelector">A function that specifies how to merge the elements from the two sequences.</param>
        /// <typeparam name="TSecond">The type of the elements of the second input sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements of the result sequence.</typeparam>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains merged elements of two input sequences.</returns>
        public IEnumerable<TResult> Zip<TSecond, TResult>(IEnumerable<TSecond> second,
                                                          Func<ObjectPair<TK, TV>, TSecond, TResult> resultSelector) =>
            SerializedList.Zip(second, resultSelector);

        #endregion
    }
}
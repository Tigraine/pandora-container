using System;
using System.Collections;
using System.Collections.Generic;

namespace Pandora
{
    public class CollidingDictionary<TKey, TValue>
        where TKey : IEquatable<TKey>
    {
        private IDictionary<TKey, IList<TValue>> dictionary = new Dictionary<TKey, IList<TValue>>();
        public void Add(TKey key, TValue value)
        {
            if (!dictionary.ContainsKey(key))
                dictionary.Add(key, new List<TValue>());
            dictionary[key].Add(value);
        }

        public IList<TValue> Get(TKey key)
        {
            return dictionary[key];
        }

        public IList<TValue> this[TKey key]
        {
            get { return Get(key); }
        }
    }
}
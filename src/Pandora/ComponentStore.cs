using System;
using System.Collections.Generic;

namespace Pandora
{
    public class ComponentStore : IComponentStore
    {
        private IDictionary<Type, Type> store = new Dictionary<Type, Type>();
        public void Add<T, TType>() where T : class where TType : T
        {
            var type = typeof(T);
            if (store.ContainsKey(type))
            {
                throw new InvalidOperationException("Type " + type.FullName + " was already registered");
            }
            store.Add(type, typeof(TType));
        }

        public Type Get<T>() where T : class
        {
            return store[typeof (T)];
        }
    }
}
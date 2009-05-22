/*
 * Copyright 2009 Daniel Hölbling - http://www.tigraine.at
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *  http://www.apache.org/licenses/LICENSE-2.0
 *  
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;

namespace Pandora
{
    public class ComponentStore : IComponentStore
    {
        private CollidingDictionary<Type, Type> store = new CollidingDictionary<Type, Type>();
        public void Add<T, TType>() where T : class where TType : T
        {
            var type = typeof(T);
/*            if (store.ContainsKey(type))
            {
                throw new InvalidOperationException("Type " + type.FullName + " was already registered");
            }*/
            store.Add(type, typeof(TType));
        }

        public IList<Type> Get<T>() where T : class
        {
            return Get(typeof (T));
        }

        public IList<Type> Get(Type type)
        {
            return store[type];
        }
    }
}
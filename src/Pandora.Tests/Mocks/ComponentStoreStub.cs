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
using System.Reflection;

namespace Pandora.Tests.Mocks
{
    public class ComponentStoreStub : IComponentStore
    {
        IDictionary<string, int> callsToMethod = new Dictionary<string, int>();

        public int GetCallCount(string methodName)
        {
            if (callsToMethod.ContainsKey(methodName))
                return callsToMethod[methodName];
            return 0;
        }

        private IList<Type> getResult = new List<Type>();

        public void AddResultForGet(Type result)
        {
            getResult.Add(result);
        }

        public void Add<T, TType>() where T : class where TType : T
        {
            var methodName = MethodBase.GetCurrentMethod().Name;
            CountMethodCall(methodName);
        }

        private void CountMethodCall(string methodName)
        {
            if (!callsToMethod.ContainsKey(methodName))
                callsToMethod.Add(methodName, 0);
            callsToMethod[methodName]++;
        }

        public Type Get<T>() where T : class
        {
            var methodName = MethodBase.GetCurrentMethod().Name;
            CountMethodCall(methodName);

            var result = getResult[0];
            getResult.RemoveAt(0);

            return result;
        }

        public Type Get(Type type)
        {
            throw new NotImplementedException();
        }
    }
}
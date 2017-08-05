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

namespace PandoraContainer.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Fluent;
    public class ContainerMock : IPandoraContainer
    {
        public delegate object Expectation();

        private IDictionary<string, Expectation> expectations = new Dictionary<string, Expectation>();

        public void AddExpectation(string methodName, Expectation expectation)
        {
            expectations.Add(methodName, expectation);
        }

        private object CallExpectation(string methodName, bool isGeneric)
        {
            var key = methodName;
            if (isGeneric)
            {
                key = "Generic" + methodName;
            }
            if (expectations.ContainsKey(key))
                return expectations[key]();
            throw new InvalidOperationException("No expectation found for method " + methodName);
        }

        public void AddComponent<T, TImplementor>() where T : class where TImplementor : T
        {
            CallExpectation(nameof(AddComponent), true);
        }

        public T Resolve<T>()
        {
            return (T)CallExpectation(nameof(Resolve), true);
        }

        public object Resolve(Type type)
        {
            return CallExpectation(nameof(Resolve), false);
        }

        public T Resolve<T>(string name)
        {
            return (T)CallExpectation(nameof(Resolve), true);
        }

        public object Resolve(Type type, string name)
        {
            return CallExpectation(nameof(Resolve), false);
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> ResolveAll(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public void Register(Action<FluentRegistration> registrationClosure)
        {
            throw new NotImplementedException();
        }
    }
}
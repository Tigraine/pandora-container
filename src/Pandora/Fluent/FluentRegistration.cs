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

namespace Pandora.Fluent
{
    public class FluentRegistration
    {
        private readonly ComponentStore store;
        internal IRegistration componentRegistration = new Registration();

        public FluentRegistration(ComponentStore store)
        {
            this.store = store;
        }

        public FluentServiceOptions<T> Service<T>()
        {
            return Service<T>(null);
        }

        public FluentServiceOptions<T> Service<T>(string name)
        {
            componentRegistration = new Registration { Name = name, Service = typeof(T) };
            store.AddRegistration(componentRegistration);
            return new FluentServiceOptions<T>(this);
        }
    }
}
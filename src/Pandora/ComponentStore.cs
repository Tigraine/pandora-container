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

namespace Pandora
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ComponentStore : IComponentStore
    {
        private IList<IRegistration> registrations = new List<IRegistration>();

        public virtual void Add<T, TType>() where T : class where TType : T
        {
            Add<T, TType>(null);
        }
        public virtual void Add<T, TType>(string name) where T : class where TType : T
        {
            var registration = new Registration
                                   {
                                       Service = typeof (T),
                                       Implementor = typeof (TType),
                                       Name = name
                                   };
            registrations.Add(registration);
        }

        public virtual IList<IRegistration> GetRegistrationsForService<T>() where T : class
        {
            return GetRegistrationsForService(typeof (T));
        }

        public virtual IList<IRegistration> GetRegistrationsForService(Type type)
        {
            var service = registrations.Where(p => p.Service == type).ToList();
            if (service.Count == 0)
                throw new KeyNotFoundException(String.Format("No Component implementing {0} could be found", type.FullName));
            return service;
        }
    }
}
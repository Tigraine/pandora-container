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
    using Fluent;

    public class ComponentStore : IComponentStore
    {
        private readonly IList<IRegistration> registrations = new List<IRegistration>();

        public virtual IRegistration Add<T, TType>() where TType : T
        {
            return Add<T, TType>(null);
        }
        public virtual IRegistration Add<T, TType>(string name) where TType : T
        {
            var registration = new Registration
            {
                Service = typeof(T),
                Implementor = typeof(TType),
                Name = name
            };
            AddRegistration(registration);
            return registration;
        }

        internal void AddRegistration(IRegistration registration)
        {
            if (registrations.Any(p => p.Name == registration.Name && p.Name != null)) 
                throw new NameAlreadyRegisteredException(registration.Name);
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

        public void Register(Action<FluentRegistration> registrationClosure)
        {
            registrationClosure(new FluentRegistration(this));
        }

        public IRegistration AddInstance<T>(string name, T instance)
        {
            var add = Add<T, T>(name);
            new RegistrationWriter(add).SetInstance(instance);
            return add;
        }

        public IRegistration AddInstance<T>(T instance)
        {
            return AddInstance(null, instance);
        }
    }
}
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
    using Fluent;

    public class PandoraContainer : IPandoraContainer
    {
        private readonly IComponentStore componentStore;
        private readonly Resolver resolver;
        private readonly IComponentLookup lookupService;


        public PandoraContainer(IComponentStore componentStore)
        {
            this.componentStore = componentStore;
            lookupService = new ConcreteClassInstantiationLookupServiceDecorator(new LookupService(componentStore));
            var activator = new ComponentActivator();
            resolver = new Resolver(activator, lookupService);
        }
       
        public virtual T Resolve<T>()
        {
            return (T)Resolve(typeof (T));
        }

        public virtual object Resolve(Type type)
        {
            return Resolve(type, null);
        }

        public virtual T Resolve<T>(string name)
        {
            return (T) Resolve(typeof (T), name);
        }

        public virtual object Resolve(Type type, string name)
        {
            var query = new Query { ServiceType = type, Name = name };
            return resolver.CreateType(query);
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            foreach(var  service in ResolveAll(typeof(T)))
            {
                yield return (T) service;
            }
        }

        public IEnumerable<object> ResolveAll(Type serviceType)
        {
            var registrations = componentStore.GetRegistrationsForService(serviceType);
            foreach(var item in registrations)
            {
                object resolve = null;
                try
                {
                    resolve = Resolve(item.Service);
                }
                catch
                {}
                if (resolve != null)
                    yield return resolve;
            }
        }

        public void Register(Action<FluentRegistration> registrationClosure)
        {
            componentStore.Register(registrationClosure);
        }
    }
}
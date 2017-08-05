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

namespace PandoraContainer
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class ConcreteClassInstantiationLookupServiceDecorator : IComponentLookup
    {
        private readonly IComponentLookup underlying;
        private readonly ILifestyle lifestyle;
        private readonly IDictionary<Type, IRegistration> createdRegistrations = new Dictionary<Type, IRegistration>();

        public ConcreteClassInstantiationLookupServiceDecorator(IComponentLookup underlying, ILifestyle lifestyle)
        {
            this.underlying = underlying;
            this.lifestyle = lifestyle;
        }

        public IRegistration LookupType(Query targetType, ResolverContext context)
        {
            IRegistration registration = underlying.LookupType(targetType, context);
            if (registration != null)
            {
                return registration;
            }

            Type type = targetType.ServiceType;
#if NET35
            if (!type.IsInterface && !type.IsAbstract && targetType.Name == null)
#else
            if (!type.GetTypeInfo().IsInterface && !type.GetTypeInfo().IsAbstract && targetType.Name == null)
#endif
            {
                lock (createdRegistrations)
                {
                    if (createdRegistrations.ContainsKey(type))
                        return createdRegistrations[type];
                    var reg = new Registration
                                  {
                                      Service = targetType.ServiceType,
                                      Implementor = targetType.ServiceType,
                                      Lifestyle = lifestyle
                                  };
                    createdRegistrations.Add(type, reg);
                    return reg;
                }
            }
            return null;
        }
    }
}
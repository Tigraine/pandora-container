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

    public class ConcreteClassInstantiationLookupServiceDecorator : IComponentLookup
    {
        private readonly IComponentLookup underlying;
        private readonly IDictionary<Type, IRegistration> createdRegistrations = new Dictionary<Type, IRegistration>();

        public ConcreteClassInstantiationLookupServiceDecorator(IComponentLookup underlying)
        {
            this.underlying = underlying;
        }

        public IRegistration LookupType(Query targetType, ResolverContext context)
        {
            try 
            {
                IRegistration registration = underlying.LookupType(targetType, context);
                return registration;
            }
            catch (ServiceNotFoundException)
            {
                Type type = targetType.ServiceType;
                if (!type.IsInterface && !type.IsAbstract && targetType.Name == null)
                {
                    if (createdRegistrations.ContainsKey(type))
                        return createdRegistrations[type];
                    var registration = new Registration
                                           {
                                               Service = targetType.ServiceType,
                                               Implementor = targetType.ServiceType
                                           };
                    createdRegistrations.Add(type, registration);
                    return registration;
                }
                throw;
            }
        }
    }
}
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
    using Model;

    public class Resolver
    {
        private readonly IComponentActivator activator;
        private readonly IComponentLookup componentLookup;

        public Resolver(IComponentActivator activator, IComponentLookup componentLookup)
        {
            this.activator = activator;
            this.componentLookup = componentLookup;
        }

        private IRegistration ImplementorLookup(Type targetType, ResolverContext context)
        {
            return componentLookup.LookupType(targetType, context);
        }

        private CreationContext CreateReturnContext(Type type, object[] parameters)
        {
            return new CreationContext
            {
                ConcreteType = type,
                Arguments = parameters
            };
        }

        public CreationContext GenerateCreationContext(Type componentType, ResolverContext context)
        {
            var constructors = componentType.GetConstructors()
                .OrderByDescending(p => p.GetParameters().Count());

            IList<DependencyMissingException> missingDependencies = new List<DependencyMissingException>();
            foreach (var info in constructors)
            {
                missingDependencies = new List<DependencyMissingException>();
                var parameters = info.GetParameters();
                if (parameters.Length == 0) //Fast way out.
                    return CreateReturnContext(componentType, null);

                IList<object> resolvedParameters = new List<object>();
                foreach (var parameter in parameters)
                {
                    Type type = parameter.ParameterType;

                    try
                    {
                        resolvedParameters.Add(CreateType(type, context));
                    }
                    catch (ServiceNotFoundException exception)
                    {
                        missingDependencies.Add(new DependencyMissingException(exception.Message));
                    }
                }
                if (resolvedParameters.Count == parameters.Length)
                    return CreateReturnContext(componentType, resolvedParameters.ToArray());
            }
            throw missingDependencies.First(); //TODO: Expose all missing dependencies
        }

        public object CreateType(Type targetType, ResolverContext context)
        {
            //Need to create a deep copy of the Context to make splitting the graph possible
            var localContext = ResolverContext.CreateContextFromContext(context);
            var registration = ImplementorLookup(targetType, localContext);
            Type componentType = registration.Implementor;

            var creationContext = GenerateCreationContext(componentType, localContext);
            return activator.CreateInstance(creationContext);
        }

        public object CreateType(Type serviceType)
        {
            return CreateType(serviceType, new ResolverContext());
        }
    }
}
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

        private IRegistration FindSuitableImplementor(Query query, ResolverContext context)
        {
            return componentLookup.LookupType(query, context);
        }

        private CreationContext CreateReturnContext(Type type, object[] parameters)
        {
            return new CreationContext
            {
                ConcreteType = type,
                Arguments = parameters
            };
        }

        public virtual CreationContext GenerateCreationContext(IRegistration registration, ResolverContext context)
        {
            var componentType = registration.Implementor;
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
                    var dependencyName = registration.Parameters(parameter.Name).ParameterValue;
                    Type type = parameter.ParameterType;

                    try
                    {
                        var query = new Query {ServiceType = type, Name = dependencyName};
                        resolvedParameters.Add(CreateType(query, context));
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

        public virtual object CreateType(Query query, ResolverContext context)
        {
            //Need to create a deep copy of the Context to make splitting the graph possible
            var localContext = ResolverContext.CreateContextFromContext(context);
            var registration = FindSuitableImplementor(query, localContext);

            var creationContext = GenerateCreationContext(registration, localContext);
            return activator.CreateInstance(creationContext);
        }

        public virtual object CreateType(Query query)
        {
            return CreateType(query, new ResolverContext());
        }
    }

    public class Query
    {
        public Type ServiceType { get; set; }
        public string Name { get; set; }
    }
}
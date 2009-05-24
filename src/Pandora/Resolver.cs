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
using System.Linq;

namespace Pandora
{
    public class Resolver
    {
        private readonly IComponentStore componentStore;

        public Resolver(IComponentStore componentStore)
        {
            this.componentStore = componentStore;
        }

        private IRegistration SkipParents(IList<IRegistration> candidates, IList<IRegistration> parents)
        {
            foreach (var candidate in candidates)
            {
                if (!parents.Contains(candidate)) return candidate;
            }
            throw new KeyNotFoundException();
        }

        private object ActivateInstance(Type type, object[] parameters)
        {
            return Activator.CreateInstance(type, parameters);
        }

        public object ActivateType(Type targetType, IList<IRegistration> parents)
        {
            IList<IRegistration> localParents = new List<IRegistration>(parents);
            Type componentType;
            try
            {
                var registration = SkipParents(componentStore.GetRegistrationsForService(targetType), localParents);
                componentType = registration.Implementor;
                localParents.Add(registration);
            }
            catch (KeyNotFoundException)
            {
                throw new ServiceNotFoundException(targetType.FullName);
            }

            var constructors = componentType.GetConstructors()
                .OrderByDescending(p => p.GetParameters().Count());

            IList<DependencyMissingException> missingDependencies = new List<DependencyMissingException>();
            foreach (var info in constructors)
            {
                missingDependencies = new List<DependencyMissingException>();
                var parameters = info.GetParameters();
                if (parameters.Length == 0) //Fast way out.
                    return ActivateInstance(componentType, null);

                IList<object> resolvedParameters = new List<object>();
                foreach (var parameter in parameters)
                {
                    Type type = parameter.ParameterType;

                    try
                    {
                        resolvedParameters.Add(ActivateType(type, localParents));
                    }
                    catch (ServiceNotFoundException exception)
                    {
                        missingDependencies.Add(new DependencyMissingException(exception.Message));
                    }
                }
                if (resolvedParameters.Count == parameters.Length)
                    return ActivateInstance(componentType, resolvedParameters.ToArray());
            }
            if (missingDependencies.Count > 0)
                throw missingDependencies.First(); //TODO: Expose all missing dependencies


            //Need to implement errormessage
            throw new NotImplementedException();
        }
    }
}
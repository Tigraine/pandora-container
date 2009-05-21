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
using System.Reflection;

namespace Pandora
{
    public class PandoraContainer
    {
        private readonly IComponentStore componentStore;

        public PandoraContainer(IComponentStore componentStore)
        {
            this.componentStore = componentStore;
        }

        public void AddComponent<T, TImplementor>()
            where T : class
            where TImplementor : T
        {
            componentStore.Add<T, TImplementor>();
        }

        public T Resolve<T>()
            where T : class
        {
            Type componentType;
            try
            {
                componentType = componentStore.Get<T>();
            }
            catch (KeyNotFoundException)
            {
                throw new ServiceNotFoundException(typeof (T).FullName);
            }

            var constructors = componentType.GetConstructors();
            var enumerable = constructors.OrderByDescending(p => p.GetParameters().Count());

            foreach (var info in enumerable)
            {
                var parameters = info.GetParameters();
                if (parameters.Length == 0) //Fast way out.
                    return (T) Activator.CreateInstance(componentType);

                IList<object> resolvedParameters = new List<object>();
                foreach (var parameter in parameters)
                {
                    Type type = parameter.ParameterType;
                    MethodInfo method = typeof (PandoraContainer).GetMethod("Resolve");
                    MethodInfo generic = method.MakeGenericMethod(type);
                    try
                    {
                        resolvedParameters.Add(generic.Invoke(this, null));
                    }
                    catch (TargetInvocationException exception)
                    {
                        if (exception.InnerException is ServiceNotFoundException)
                        {
                            throw new DependencyMissingException(exception.InnerException.Message);
                        }
                        throw exception.InnerException;
                    }
                }
                if (resolvedParameters.Count == parameters.Length)
                    return (T) Activator.CreateInstance(componentType, resolvedParameters.ToArray());
            }


            //Need to implement errormessage
            throw new NotImplementedException();
        }
    }
}
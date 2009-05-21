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

            IList<ServiceNotFoundException> exceptions;

            foreach (var info in enumerable)
            {
                exceptions = new List<ServiceNotFoundException>();

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

    public class DependencyMissingException : ApplicationException
    {
        public DependencyMissingException(string name)
            : base (String.Format("Service could not be created because one of it's dependencies are missing:{0}{1}", Environment.NewLine, name))
        {
            
        }
    }
}
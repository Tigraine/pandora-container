using System;
using System.Linq;

namespace Pandora
{
    public interface IComponentStore
    {
        void Add<T, TType>()
            where T : class
            where TType : T;

        Type Get<T>()
            where T : class;
    }

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
            var componentType = componentStore.Get<T>();
            var constructors = componentType.GetConstructors();
            var enumerable = constructors.OrderBy(p => p.GetParameters().Count());
            foreach (var info in enumerable)
            {
                if (info.GetParameters().Length == 0)
                    return (T)Activator.CreateInstance(componentType);
            }
            //Need to implement errormessage
            throw new NotImplementedException();
        }
    }
}
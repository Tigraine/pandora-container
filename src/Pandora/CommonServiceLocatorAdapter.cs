using System;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;

namespace Pandora
{
    public class CommonServiceLocatorAdapter : IServiceLocator
    {
        private readonly PandoraContainer container;

        public CommonServiceLocatorAdapter(PandoraContainer container)
        {
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public object GetInstance(Type serviceType)
        {
            return container.Resolve(serviceType);
        }

        public object GetInstance(Type serviceType, string key)
        {
            throw new NotImplementedException("Key lookups are not supported by Pandora");
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public TService GetInstance<TService>()
        {
            return container.Resolve<TService>();
        }

        public TService GetInstance<TService>(string key)
        {
            throw new NotImplementedException("Key lookups are not supported by Pandora");
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            throw new NotImplementedException();
        }
    }
}
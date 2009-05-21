using System;

namespace Pandora
{
    public class ServiceNotFoundException : ApplicationException
    {
        public ServiceNotFoundException(string name)
            : base(String.Format("No service for key {0} was found", name))
        {
        }
    }
}
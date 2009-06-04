using System;

namespace Pandora
{
    public class LifestyleConfiguration
    {
        private readonly IRegistration registration;

        public LifestyleConfiguration(IRegistration registration, Action<ILifestyle> lifestyleSetDelegate)
        {
            this.registration = registration;
        }

        public IRegistration Singleton()
        {
            return registration;
        }
        public IRegistration Transient()
        {
            return registration;
        }
    }
       
    public interface ILifestyle
    {
        
    }
    public class TransientLifestyle
    {
        
    }
    public class SingletonLifestyle
    {
        
    }
}
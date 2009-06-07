namespace Pandora
{
    using System;

    public class RegistrationException : ApplicationException
    {
        public RegistrationException(Type implementorType)
            :base(String.Format("Cannot register Interface {0} as implementing class", implementorType.Name))
        {
            
        }
    }
}
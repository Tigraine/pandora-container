namespace PandoraContainer
{
    using System;

    public class RegistrationException :
#if NET35
        ApplicationException
#else
        Exception
#endif
    {
        public RegistrationException(Type implementorType)
            :base(String.Format("Cannot register Interface {0} as implementing class", implementorType.Name))
        {
            
        }
    }
}
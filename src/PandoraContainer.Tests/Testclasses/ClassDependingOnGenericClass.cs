namespace PandoraContainer.Tests.Testclasses
{
    public class ClassDependingOnGenericClass : IService
    {
        private readonly GenericClass<string> genericClass;

        public ClassDependingOnGenericClass(GenericClass<string> genericClass)
        {
            this.genericClass = genericClass;
        }
    }
}
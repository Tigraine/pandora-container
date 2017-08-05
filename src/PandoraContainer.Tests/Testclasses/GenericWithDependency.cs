namespace PandoraContainer.Tests.Testclasses
{
    public class GenericWithDependency<T>
    {
        private readonly T dependency;

        public GenericWithDependency(T dependency)
        {
            this.dependency = dependency;
        }

        public T Dependency
        {
            get
            {
                return dependency;
            }
        }
    }
}
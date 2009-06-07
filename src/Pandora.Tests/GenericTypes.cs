namespace Pandora.Tests
{
    using Testclasses;
    using Xunit;

    public class GenericTypes
    {
        private ComponentStore store;
        private PandoraContainer container;

        public GenericTypes()
        {
            store = new ComponentStore();
            container = new PandoraContainer(store);
        }

        [Fact]
        public void CanResolveSpecificGenericClass()
        {
            store.Register(p => 
                p.Service<GenericClass<string>>()
                .Implementor<GenericClass<string>>());

            Assert.DoesNotThrow(() => container.Resolve<GenericClass<string>>());
        }
    }
}
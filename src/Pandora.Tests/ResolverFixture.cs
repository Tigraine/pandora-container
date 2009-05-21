using Pandora.Tests.Mocks;
using Pandora.Tests.Testclasses;
using Rhino.Mocks;
using Xunit;

namespace Pandora.Tests
{
    public class ResolverFixture
    {
        [Fact]
        public void CanRegisterComponent()
        {
            ComponentStoreStub componentStore = new ComponentStoreStub();

            var locator = new PandoraContainer(componentStore);
            
            locator.AddComponent<IService, ClassWithNoDependencies>();

            var callCount = componentStore.GetCallCount("Add");
            Assert.Equal(1, callCount);
        }

        [Fact]
        public void CanResolveClassWithoutDependencies()
        {
            ComponentStoreStub componentStore = new ComponentStoreStub();
            componentStore.SetResultForGet(typeof(ClassWithNoDependencies));

            PandoraContainer locator = new PandoraContainer(componentStore);
            var result = locator.Resolve<IService>();

            Assert.IsType<ClassWithNoDependencies>(result);
        }
    }
}
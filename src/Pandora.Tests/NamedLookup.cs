using Pandora.Tests.Testclasses;
using Xunit;

namespace Pandora.Tests
{
    public class NamedLookup
    {
        [Fact]
        public void CanRetrieveServiceByName()
        {
            var store = new ComponentStore();
            //This registration is to give the container something to choose from, in case named lookup won't work
            store.Add<IService, ClassWithDependencyOnItsOwnService>("memory.repository");
            store.Add<IService, ClassWithNoDependencies>("db.repository");
            var container = new PandoraContainer(store);

            var service = container.Resolve<IService>("db.repository");
            Assert.IsType<ClassWithNoDependencies>(service);
        }

        [Fact]
        public void RetrievalByNameThrowsExceptionWhenNameNotRegistered()
        {
            var store = new ComponentStore();
            var container = new PandoraContainer(store);
            //To make it lookup something
            container.AddComponent<ClassWithNoDependencies, ClassWithNoDependencies>();

            Assert.Throws<ServiceNotFoundException>(() => container.Resolve<ClassWithNoDependencies>("test"));
        }
    }

    public class ExplicitDependencies
    {
        [Fact]
        public void CanSpecifyDependencyByName()
        {
            var store = new ComponentStore();
            store.Add<IService, ClassWithNoDependencies>("service1");
            store.Add<IService, ClassWithNoDependencies2>("service2");
            store.Add<ClassWithOneDependency, ClassWithOneDependency>()
                .Parameters("dependency").Eq("service2");
            var container = new PandoraContainer(store);

            var service2 = container.Resolve<ClassWithOneDependency>();

            Assert.IsType<ClassWithNoDependencies2>(service2.Dependency);
        }
    }
}
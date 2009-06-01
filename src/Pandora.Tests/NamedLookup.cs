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
    }
}
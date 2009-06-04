using Pandora.Tests.Testclasses;
using Xunit;

namespace Pandora.Tests
{
    public class Lifestyles
    {
        [Fact]
        public void SingletonLifestyleOnlyCreatesComponentOnce()
        {
            var store = new ComponentStore();
            store.Add<IService, ClassWithNoDependencies>()
                .Lifestyle.Singleton();

            var container = new PandoraContainer(store);
            var service = container.Resolve<IService>();
            var service2 = container.Resolve<IService>();

            Assert.Same(service, service2);
        }
    }
}
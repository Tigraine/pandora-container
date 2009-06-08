namespace Pandora.Tests
{
    using System;
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

        [Fact]
        public void CanAutoResolveGenericTypes()
        {

            store.Register(
                p => p.Generic(typeof (GenericClass<>))
                         .Implementor(typeof (GenericClass<>))
                         .ForTypes(typeof (string), typeof (int)));

            Assert.DoesNotThrow(() =>
                                    {
                                        container.Resolve<GenericClass<string>>();
                                        container.Resolve<GenericClass<int>>();
                                    });
            Assert.Throws<ServiceNotFoundException>(() => container.Resolve<GenericClass<long>>());
        }

        [Fact]
        public void RegisterNonGenericClassAsServiceThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => store.Register(
                                                       p => p.Generic(typeof(IService))));
        }

        [Fact]
        public void RegisterNonGenericClassAsImplementorThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => store.Register(
                                                       p => p.Generic(typeof(GenericClass<>))
                                                       .Implementor(typeof(ClassWithNoDependencies))));
        }
    }
}
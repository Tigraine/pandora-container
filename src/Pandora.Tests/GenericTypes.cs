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
                         .OnlyForTypes(typeof (string), typeof (int)));

            Assert.DoesNotThrow(() =>
                                    {
                                        container.Resolve<GenericClass<string>>();
                                        container.Resolve<GenericClass<int>>();
                                    });
            //Since ConcreteClassInstantiation is active this can't be tested
            //Assert.Throws<ServiceNotFoundException>(() => container.Resolve<GenericClass<long>>());
        }

        [Fact]
        public void RegisterNonGenericClassAsServiceThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => store.Register(
                                                       p => p.Generic(typeof (IService))));
        }

        [Fact]
        public void RegisterNonGenericClassAsImplementorThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => store.Register(
                                                       p => p.Generic(typeof (GenericClass<>))
                                                                .Implementor(typeof (ClassWithNoDependencies))));
        }

        [Fact(Skip="Not implemented yet")]
        public void CanRegisterAndResolveRealGenericRequests()
        {
            store.Register(p =>
                           p.Generic(typeof (GenericClass<>))
                               .Implementor(typeof (GenericClass<>))
                               .ForAllTypes());

            Assert.DoesNotThrow(() => { var resolve = container.Resolve<GenericClass<string>>(); });
        }

        [Fact(Skip = "Not implemented yet")]
        public void CanResolveRealGenericAsSubdependency()
        {
            store.Register(p =>
                               {
                                   p.Service<IService>()
                                       .Implementor<ClassDependingOnGenericClass>();
                                   p.Generic(typeof (GenericClass<>))
                                       .Implementor(typeof (GenericClass<>))
                                       .ForAllTypes();
                               });
            Assert.DoesNotThrow(() => container.Resolve<IService>());
        }

        [Fact(Skip = "Not implemented yet")]
        public void CanResolveDependenciesOfGenericType()
        {
            store.Register(p =>
                               {
                                   p.Generic(typeof (GenericWithDependency<>))
                                       .Implementor(typeof (GenericWithDependency<>))
                                       .ForAllTypes();
                                   p.Service<string>()
                                       .Instance("Hello World");
                               });

            var result = container.Resolve<GenericWithDependency<string>>();
            Assert.Equal("Hello World", result.Dependency);
        }
    }
}
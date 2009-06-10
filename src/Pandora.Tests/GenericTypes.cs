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
            Assert.Throws<ServiceNotFoundException>(() => container.Resolve<GenericClass<long>>());
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

        [Fact]
        public void CanRegisterAndResolveRealGenericRequests()
        {
            store.Register(p =>
                           p.Generic(typeof (GenericClass<>))
                               .Implementor(typeof (GenericClass<>))
                               .ForAllTypes());

            Assert.DoesNotThrow(() => { var resolve = container.Resolve<GenericClass<string>>(); });
        }

        [Fact]
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

        [Fact]
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

        [Fact]
        public void GenericTests()
        {
            var type = typeof (GenericClass<>);
            var type2 = typeof (GenericClass<string>);
            var type3 = typeof (string);

            PrintTypeInfo(type);
            PrintTypeInfo(type2);
            PrintTypeInfo(type3);
        }

        private void PrintTypeInfo(Type type)
        {
            Console.WriteLine("Fullname: {0}", type.FullName);
            Console.WriteLine("IsGenericType: {0}", type.IsGenericType);
            Console.WriteLine("IsGenericTypeDefinition: {0}", type.IsGenericTypeDefinition);
            foreach (var t in type.GetGenericArguments())
            {
                Console.WriteLine(t.Name);
            }
            Console.WriteLine("----");
        }
    }
}
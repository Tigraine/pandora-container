/*
 * Copyright 2009 Daniel Hölbling - http://www.tigraine.at
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *  http://www.apache.org/licenses/LICENSE-2.0
 *  
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace Pandora.Tests
{
    using System.Linq;
    using Testclasses;
    using Xunit;

    public class FluentInterface
    {
        [Fact]
        public void CanRegisterMultipleParametersInARow()
        {
            var store = new ComponentStore();
            store.Register(p => p.Service<IService>()
                                    .Implementor<ClassWithNoDependencies>()
                                    .Parameters("test").Set("test")
                                    .Parameters("repository").Set("something"));


            var registrations = store.GetRegistrationsForService<IService>().First();
            Assert.NotNull(registrations.Parameters["test"]);
            Assert.NotNull(registrations.Parameters["repository"]);
        }

        [Fact]
        public void CanConfigureLifestyle()
        {
            var store = new ComponentStore();
            store.Register((p) =>
                               {
                                   p.Service<IService>()
                                       .Implementor<ClassWithDependencyOnItsOwnService>()
                                       .Lifestyle.Singleton();
                                   p.Service<IService>()
                                       .Implementor<ClassWithNoDependencies>()
                                       .Lifestyle.Singleton();
                               });
            var container = new PandoraContainer(store);
            Assert.DoesNotThrow(() => container.Resolve<IService>());
        }

        [Fact]
        public void CanConfigureComponentWithoutSpecifyingLifestyleAndParameters()
        {
            var store = new ComponentStore();
            Assert.DoesNotThrow(() => store.Register((p) => p.Service<IService>()
                                                                .Implementor<ClassWithNoDependencies>()));
            var container = new PandoraContainer(store);
            Assert.DoesNotThrow(() => container.Resolve<IService>());
        }

        [Fact]
        public void CanSpecifyParametersThroughFluentInterface()
        {
            var store = new ComponentStore();
            store.Register(p =>
                               {
                                   p.Service<ClassWithDependencyOnItsOwnService>("myService")
                                       .Implementor<ClassWithDependencyOnItsOwnService>()
                                       .Parameters("service").Set("service1");
                                   p.Service<IService>("service1")
                                       .Implementor<ClassWithNoDependencies>();
                                   p.Service<IService>("service2")
                                       .Implementor<ClassWithTwoDependenciesOnItsOwnService>();
                               });

            var container = new PandoraContainer(store);
            var service = container.Resolve<ClassWithDependencyOnItsOwnService>();
            Assert.IsType<ClassWithNoDependencies>(service.SubService);
        }

        [Fact]
        public void CanRegisterTwoServicesWithDifferentNames()
        {
            var store = new ComponentStore();
            Assert.DoesNotThrow(() => store.Register(p =>
                                                         {
                                                             p.Service<IService>("service1")
                                                                 .Implementor<ClassWithNoDependencies>();
                                                             p.Service<IService>("service2")
                                                                 .Implementor<ClassWithNoDependencies>();
                                                         }));
        }

        [Fact]
        public void ThrowsExceptionWhenRegisteringTwoServicesWithSameName()
        {
            var store = new ComponentStore();
            Assert.Throws<NameAlreadyRegisteredException>(() => store.Register(p =>
                                                                                   {
                                                                                       p.Service<IService>("service1")
                                                                                           .Implementor
                                                                                           <ClassWithNoDependencies>();
                                                                                       p.Service<IService>("service1")
                                                                                           .Implementor
                                                                                           <ClassWithNoDependencies>();
                                                                                   }));
        }

        [Fact]
        public void CanSpecifyCustomLifestyle()
        {
            var myLifestyle = new CustomLifestyle();
            var store = new ComponentStore();
            store.Register(p => p.Service<IService>()
                                    .Implementor<ClassWithNoDependencies>()
                                    .Lifestyle.Custom(myLifestyle));

            Assert.IsType<CustomLifestyle>(store.GetRegistrationsForService<IService>().First().Lifestyle);
        }

        [Fact]
        public void CanInjectInstanceThroughFluentConfiguration()
        {
            var store = new ComponentStore();
            var instance = new ClassWithNoDependencies();
            store.Register(p => p.Service<IService>("test")
                .Instance(instance));

            //TODO: Maybe clean this up. Too much implementation detail
            var execute = store.GetRegistrationsForService(typeof(IService)).First().Lifestyle.Execute(null);
            Assert.Same(instance, execute);
        }
    }
}
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
    using System.Collections.Generic;
    using Testclasses;
    using Xunit;

    public class ComponentStoreFixture
    {
        [Fact]
        public void CanInsertRegistration()
        {
            var store = new ComponentStore();

            Assert.DoesNotThrow(() => 
                    store.Add<IService, ClassWithNoDependencies>()
                );

            var registration = store.GetRegistrationsForService<IService>()[0];
            Assert.Equal(registration.Implementor, typeof(ClassWithNoDependencies));
        }

        [Fact]
        public void CanInsertServiceTwice()
        {
            var store = new ComponentStore();
            store.Add<IService, ClassWithNoDependencies>();

            Assert.DoesNotThrow(() => store.Add<IService, ClassWithNoDependencies>());
        }

        [Fact]
        public void ReturnsNullWhenKeyNotFound()
        {
            var store = new ComponentStore();

            var registrationsForService = store.GetRegistrationsForService<IService>();
            Assert.Equal(0, registrationsForService.Count);
        }

        [Fact]
        public void CannotInsertOneNameTwice()
        {
            var store = new ComponentStore();
            var name = "test";
            store.Add<IService, ClassWithNoDependencies>(name);
            Assert.Throws<NameAlreadyRegisteredException>(() => store.Add<IService, ClassWithNoDependencies>(name));
        }

        [Fact]
        public void CannotInsertInterfaceAsImplementor()
        {
            var store = new ComponentStore();
            Assert.Throws<RegistrationException>(() => store.Add<IService, IService>());
        }
    }
}
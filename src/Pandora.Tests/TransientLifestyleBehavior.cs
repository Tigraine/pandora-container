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
    using Testclasses;
    using Xunit;
    using Lifestyles;

    public class TransientLifestyleBehavior
    {
        [Fact]
        public void ActivationHappensEveryTime()
        {
            var store = new ComponentStore();
            var registration = store.Add<IService, ClassWithNoDependencies>();
            registration.Lifestyle = ComponentLifestyles.Transient;

            var container = new PandoraContainer(store);
            var service = container.Resolve<IService>();
            var service2 = container.Resolve<IService>();

            Assert.NotSame(service, service2);
        }
    }

    public class InstanceInjection
    {
        [Fact]
        public void CanInsertInstanceForGivenServiceByName()
        {
            var store = new ComponentStore();
            var instance = new ClassWithNoDependencies();
            store.AddInstance<IService>("test", instance);
            var container = new PandoraContainer(store);

            var service = container.Resolve<IService>("test");

            Assert.Same(instance, service);
        }

        [Fact]
        public void CanInsertInstanceWithoutName()
        {
            var store = new ComponentStore();
            var instance = new ClassWithNoDependencies();
            store.AddInstance<IService>(instance);
            var container = new PandoraContainer(store);

            var service = container.Resolve<IService>();

            Assert.Same(instance, service);
        }
    }
}
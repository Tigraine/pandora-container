
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

 #if NET452
using Microsoft.Practices.ServiceLocation;

namespace PandoraContainer.Tests
{
    using Xunit;
    using Testclasses;
    public class CommonServiceLocatorAdapterFixture
    {
        [Fact]
        public void GetInstanceTranslatesToResolveOnContainer()
        {
            var container = new ContainerMock();
            var weakResolveWasCalled = false;
            container.AddExpectation("Resolve", () =>
                                                    {
                                                        weakResolveWasCalled = true;
                                                        return new ClassWithNoDependencies();
                                                    });

            var adapter = new CommonServiceLocatorAdapter(container);
            adapter.GetInstance<IService>();

            Assert.True(weakResolveWasCalled);
        }

        [Fact]
        public void GetInstanceThrowsActivationExceptionWhenComponentNotFound()
        {
            var container = new ContainerMock();
            var adapter = new CommonServiceLocatorAdapter(container);
            container.AddExpectation("Resolve", () =>
                                                           {
                                                               throw new ServiceNotFoundException(typeof(IService));
            
                                                           });
            Assert.Throws<ActivationException>(() => adapter.GetInstance<IService>());
        }
    }
}
 #endif
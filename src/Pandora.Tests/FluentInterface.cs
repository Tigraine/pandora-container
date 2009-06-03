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
            store.Add<IService, ClassWithNoDependencies>()
                .Parameters("test").Set("test")
                .Parameters("repository").Set("something");

            var registrations = store.GetRegistrationsForService<IService>().First();
            Assert.NotNull(registrations.Parameters("test").ParameterValue);
            Assert.NotNull(registrations.Parameters("repository").ParameterValue);
        }
    }
}
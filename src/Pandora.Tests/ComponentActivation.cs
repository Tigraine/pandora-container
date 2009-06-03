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
    using Model;
    using Testclasses;
    using Xunit;

    public class ComponentActivation
    {
        [Fact]
        public void CanConstructObjectWithoutParameters()
        {
            var context = new CreationContext {ConcreteType = typeof (ClassWithNoDependencies)};
            IComponentActivator activator = new ComponentActivator();
            var instance = activator.CreateInstance(context);

            Assert.IsType(typeof (ClassWithNoDependencies), instance);
        }

        [Fact]
        public void CanConstructObjectWithParameters()
        {
            var stringParameter = "string argument";
            var context = new CreationContext {ConcreteType = typeof (ClassDependingOnAString), Arguments = new[] {stringParameter}};
            IComponentActivator activator = new ComponentActivator();
            var instance = (ClassDependingOnAString)activator.CreateInstance(context);

            Assert.IsType(typeof(ClassDependingOnAString), instance);
            Assert.Equal(stringParameter, instance.Dependency);
        }
    }
}
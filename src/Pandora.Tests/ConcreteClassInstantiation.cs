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

    public class ConcreteClassInstantiation
    {
        [Fact]
        public void NoRegistrationIsRequiredToInstantiateConcreteClassWithoutDependencies()
        {
            var container = new PandoraContainer(new ComponentStore());
            var dependencies = container.Resolve<ClassWithNoDependencies>();
            Assert.IsType<ClassWithNoDependencies>(dependencies);
        }

        [Fact]
        public void ClassWithDependencyOnConcreteClassResolves()
        {
            var container = new PandoraContainer(new ComponentStore());
            var dependency =
                container.Resolve<GenericWithDependency<ClassWithNoDependencies>>();
            Assert.IsType<ClassWithNoDependencies>(dependency.Dependency);
        }

        [Fact]
        public void ConcreteClassInstantiationCanBeTurnedOff()
        {
            var behaviorConfiguration = new BehaviorConfiguration
                                            {
                                                EnableImplicitTypeInstantiation = false
                                            };
            var container = new PandoraContainer(behaviorConfiguration);
            Assert.Throws<ServiceNotFoundException>(() => container.Resolve<ClassWithNoDependencies>());
        }

        [Fact]
        public void ImplicitlyInstantiatedConcreteClassHasDefaultLifestyle()
        {
            var container = new PandoraContainer(new ComponentStore());

            var c1 = container.Resolve<ClassWithNoDependencies>();
            var c2 = container.Resolve<ClassWithNoDependencies>();

            Assert.Same(c1, c2);
        }

        [Fact]
        public void ImplicitlyInstantiatedConcreteClassLifestyleCanBeConfigured()
        {
            var configuration = new BehaviorConfiguration
                                    {
                                        ImplicitTypeLifestyle = BehaviorConfiguration.Lifestyle.Transient
                                    };
            var container = new PandoraContainer(configuration);

            var c1 = container.Resolve<ClassWithNoDependencies>();
            var c2 = container.Resolve<ClassWithNoDependencies>();

            Assert.NotSame(c1, c2);
        }
    }
}
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
using System;
using System.Collections.Generic;
using Pandora.Tests.Testclasses;
using Xunit;

namespace Pandora.Tests
{
    public class ComponentStoreFixture
    {
        [Fact]
        public void CanInsertAndRetrieveByKey()
        {
            var store = new ComponentStore();
            Assert.DoesNotThrow(
                store.Add<IService, ClassWithNoDependencies>
                );

            var type = store.Get<IService>();
            Assert.Equal(type, typeof(ClassWithNoDependencies));
        }

        [Fact]
        public void CannotInsertOneKeyTwice()
        {
            var store = new ComponentStore();
            store.Add<IService, ClassWithNoDependencies>();

            Assert.Throws<InvalidOperationException>(store.Add<IService, ClassWithNoDependencies>);
        }

        [Fact]
        public void ThrowsExceptionWhenKeyNotFound()
        {
            var store = new ComponentStore();

            Assert.Throws<KeyNotFoundException>(() => store.Get<IService>());
        }
    }
}
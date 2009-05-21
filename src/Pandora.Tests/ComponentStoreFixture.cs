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
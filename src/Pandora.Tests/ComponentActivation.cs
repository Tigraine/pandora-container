using Pandora.Model;
using Pandora.Tests.Testclasses;
using Xunit;

namespace Pandora.Tests
{
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
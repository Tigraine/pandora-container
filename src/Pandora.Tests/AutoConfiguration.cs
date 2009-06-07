namespace Pandora.Tests
{
    using AutoConfigTestclasses;
    using Xunit;

    public class AutoConfigurationBehavior
    {
        [Fact(Skip = "Needs more thinking")]
        public void ShouldTakeAllConcreteClassesFromAssemblyAndCreateRegistratiosn()
        {
            var store = new ComponentStore();
            store.Register(p => p.AutoConfigure.FromAssembly(typeof(SomeClass).Assembly));

            Assert.Equal(1, store.GetRegistrationsForService<SomeClass>().Count);
        }
    }
}
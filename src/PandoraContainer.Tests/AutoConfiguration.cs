/*namespace PandoraContainer.Tests
{
    using AutoConfigTestclasses;
    using Xunit;

    public class AutoConfigurationBehavior
    {
        [Fact(Skip = "No clear definition of behavior yet")]
        public void ShouldTakeAllConcreteClassesFromAssemblyAndCreateRegistratiosn()
        {
            var store = new ComponentStore();
            store.Register(p => p.AutoConfigure.FromAssembly(typeof(SomeClass).Assembly));

            Assert.Equal(1, store.GetRegistrationsForService<SomeClass>().Count);
        }
    }
}*/
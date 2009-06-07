namespace Pandora.Tests
{
    using Testclasses;
    using Xunit;

    public class BclDependantTypeResolving
    {
        [Fact]
        public void CanInstantiateClassThatDependsOnAString()
        {
            var store = new ComponentStore();
            store.Register(
                p =>
                {
                    p.Service<ClassDependingOnAString>()
                        .Implementor<ClassDependingOnAString>()
                        .Parameters("string1");
                    p.Service<string>("string1")
                        .Instance("Hello World");
                });
            var container = new PandoraContainer(store);

            var resolve = container.Resolve<ClassDependingOnAString>();
            Assert.Equal("Hello World", resolve.Dependency);
        }
    }
}
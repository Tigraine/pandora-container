namespace Pandora.Tests.Testclasses
{
    public interface IService
    {
        
    }
    public class ClassWithNoDependencies : IService
    {
        
    }

    public interface IService2
    {
        
    }
    public class ClassWithOneDependency : IService2
    {
        private readonly IService dependency;

        public ClassWithOneDependency(IService dependency)
        {
            this.dependency = dependency;
        }
    }

    public interface IService3
    {
        
    }

    public class ClassDependingOnClassWithOneDependency : IService3
    {
        private readonly IService2 service2;

        public ClassDependingOnClassWithOneDependency(IService2 service2)
        {
            this.service2 = service2;
        }
    }
}
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
        public ClassWithOneDependency(IService dependency)
        {
            
        }
    }
}
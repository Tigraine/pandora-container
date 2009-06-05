namespace Pandora.Tests.Testclasses
{
    using System;

    public class CustomLifestyle : ILifestyle
    {
        public object Execute(Func<object> action)
        {
            return action();
        }
    }
}
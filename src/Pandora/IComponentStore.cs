using System;

namespace Pandora
{
    public interface IComponentStore
    {
        void Add<T, TType>()
            where T : class
            where TType : T;

        Type Get<T>()
            where T : class;
    }
}
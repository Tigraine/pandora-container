using System;

namespace Pandora
{
    public interface IRegistration
    {
        Guid Guid { get; }
        Type Service { get; set; }
        Type Implementor { get; set; }
        string Name { get; set; }
    }
}
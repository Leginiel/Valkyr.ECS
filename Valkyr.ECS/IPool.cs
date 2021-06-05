using System;

namespace Valkyr.ECS
{
    public interface IPool : IDisposable
    {
        bool Create(); 
        bool Store<T>(T value);
        T Receive<T>(int index);
        bool HasCapacity();
    }
}

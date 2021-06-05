using System;

namespace Valkyr.ECS
{
  public sealed class World : IWorld
  {
    private readonly int maxCapacity;

    public World(int maxCapacity = int.MaxValue)
    {
      this.maxCapacity = maxCapacity;
    }
    public ref Entity CreateEntity()
    {
      throw new NotImplementedException();
    }

    public bool Has<T>(int entityId)
    {
      throw new NotImplementedException();
    }
  }
}

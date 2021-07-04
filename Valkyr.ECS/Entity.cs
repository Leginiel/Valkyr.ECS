using System;

namespace Valkyr.ECS
{
  public class Entity
  {
    public readonly int Id;
    private readonly IWorld world;

    internal Entity(int id, IWorld world)
    {
      Id = id;
      this.world = world;
    }

    public bool Has<T>()
      where T : IComponent
    {
      return world.Has<T>(Id);
    }
    public T Get<T>()
      where T : IComponent
    {
      return world.Get<T>(Id);
    }
    public void Remove<T>()
      where T : IComponent
    {
      world.Remove<T>(Id);
    }
    public void Set<T>(in T component)
      where T : IComponent
    {
      world.Set(Id, in component);
    }
  }
}

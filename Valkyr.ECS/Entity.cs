using System;

namespace Valkyr.ECS
{
  public class Entity : IEquatable<Entity>
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
    public bool Equals(Entity other)
    {
      return Id.Equals(other.Id);
    }
    public override bool Equals(object obj)
    {
      return obj is Entity entity && Equals(entity);
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(Id);
    }

    public static bool operator ==(Entity left, Entity right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(Entity left, Entity right)
    {
      return !(left == right);
    }
  }
}

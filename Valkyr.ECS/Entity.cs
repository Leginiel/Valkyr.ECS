using System;

namespace Valkyr.ECS
{
  public readonly struct Entity : IEquatable<Entity>
  {
    public readonly int Id;

    internal Entity(int id, IWorld world)
    {
      Id = id;
    }

    public bool Has<T>()
        where T : IComponent
    {
      throw new NotImplementedException();
    }
    public bool HasComponents()
    {
      throw new NotImplementedException();
    }
    public T Get<T>()
        where T : IComponent
    {
      throw new NotImplementedException();
    }
    public bool Remove<T>()
        where T : IComponent
    {
      throw new NotImplementedException();
    }
    public bool Set<T>(in T component)
        where T : IComponent
    {
      throw new NotImplementedException();
    }
    public bool SetSameAs<T>(in Entity reference)
        where T : IComponent
    {
      throw new NotImplementedException();
    }

    public bool Equals(Entity other)
    {
      throw new NotImplementedException();

      //return Id.Equals(other.Id);
    }

    public override bool Equals(object obj)
    {
      throw new NotImplementedException();

      //return obj is Entity entity && Equals(entity);
    }

    public override int GetHashCode()
    {
      throw new NotImplementedException();
      //return HashCode.Combine(Id);
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

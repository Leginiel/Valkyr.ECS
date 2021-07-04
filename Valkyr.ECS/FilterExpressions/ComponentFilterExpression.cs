using System;

namespace Valkyr.ECS
{
  internal class ComponentFilterExpression<Component> : IFilterExpression
    where Component : IComponent
  {
    public bool Matches<T>(T element)
    {
      if (element is not Entity entity)
        throw new ArgumentException($"Element need to be of type {typeof(Entity)}", nameof(element));

      return entity.Has<Component>();
    }
  }
}

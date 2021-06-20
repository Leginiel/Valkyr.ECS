using System;

namespace Valkyr.ECS
{
  public interface IWorld : IDisposable
  {
    short Id { get; }
    bool Active { get; set; }
    ref Entity CreateEntity();
    bool Has<T>(int entityId)
      where T : IComponent;
    ref T Get<T>(int entityId)
      where T : IComponent;
    void Set<T>(int entityId, in T component)
      where T : IComponent;
    void Remove<T>(int entityId)
      where T : IComponent;
    void IterateEntities(ActionRef<Entity> entityCallback, IFilterExpression filter);
  }
}
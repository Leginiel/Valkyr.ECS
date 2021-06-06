namespace Valkyr.ECS
{
  public interface IWorld
  {
    ref Entity CreateEntity();
    bool Has<T>(int entityId)
      where T : IComponent;
    T Get<T>(int entityId)
      where T : IComponent;
    bool Set<T>(int entityId, in T component)
      where T : IComponent;
    bool Remove<T>(int entityId)
      where T : IComponent;
  }
}
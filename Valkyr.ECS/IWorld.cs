namespace Valkyr.ECS
{
  public interface IWorld
  {
    ref Entity CreateEntity();
    bool Has<T>(int entityId)
      where T : IComponent;
    ref T Get<T>(int entityId)
      where T : IComponent;
    void Set<T>(int entityId, in T component)
      where T : IComponent;
    void Remove<T>(int entityId)
      where T : IComponent;
  }
}
namespace Valkyr.ECS
{
  public interface IWorld
  {
    ref Entity CreateEntity();
    bool Has<T>(int entityId);
  }
}
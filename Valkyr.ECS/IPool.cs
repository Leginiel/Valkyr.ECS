namespace Valkyr.ECS
{
  public interface IPool
  {
    bool TryStore<T>(int id, in T value)
      where T : struct;
    bool TryReceive<T>(int id, out T result)
      where T : struct;
    bool HasCapacity();
  }
}

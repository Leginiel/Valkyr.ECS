namespace Valkyr.ECS
{
  public interface IPool<T>
    where T : struct
  {
    int Count { get; }
    ref T Store(int id, in T value);
    ref T Receive(int id);
    bool HasCapacity();
    void Remove(int id);
  }
}

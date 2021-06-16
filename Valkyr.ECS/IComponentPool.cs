namespace Valkyr.ECS
{
  public interface IComponentPool<T>
    where T : IComponent
  {
    int Count { get; }
    ref T Store(int id, in T item);
    bool Has(int id);
    ref T Receive(int id);
    bool HasCapacity();
    void Remove(int id);
  }
}

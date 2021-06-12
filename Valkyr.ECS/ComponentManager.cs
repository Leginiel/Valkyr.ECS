using System.Collections.Concurrent;

namespace Valkyr.ECS
{
  internal static class ComponentManager<T>
    where T : struct
  {
    private static readonly ConcurrentDictionary<short, IPool<T>> pools = new();

    internal static IPool<T> GetOrCreate(short worldId, int maxCapacity)
    {
      return pools.GetOrAdd(worldId, new Pool<T>(maxCapacity));
    }
  }
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Valkyr.ECS
{
  internal class Pool : IPool
  {
    private readonly int maximumCapacity;
    private readonly IDictionary storage;

    public static IPool Create<T>(int maximumCapacity = int.MaxValue)
      where T : struct
    {
      return new Pool(new Dictionary<int, T>(), maximumCapacity);
    }

    private Pool(IDictionary storage, int maximumCapacity)
    {
      this.storage = storage;
      this.maximumCapacity = maximumCapacity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool HasCapacity()
    {
      return maximumCapacity - storage.Count > 0;
    }
    public bool TryStore<T>(int id, in T item)
      where T : struct
    {
      if (!HasCapacity())
        return false;

      IDictionary<int, T> storage = GetStorage<T>();

      return storage is not null && storage.TryAdd(id, item);
    }
    public bool TryReceive<T>(int id, out T result)
      where T : struct
    {
      IDictionary<int, T> storage = GetStorage<T>();
      result = default;

      if (storage is null)
        return false;

      return storage.TryGetValue(id, out result);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private IDictionary<int, T> GetStorage<T>()
    {
      return storage as IDictionary<int, T>;
    }
  }
}

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Valkyr.ECS
{
  internal class Pool<T> : IPool<T>
    where T : struct
  {
    private const int InitialSize = 4;
    private readonly int maximumCapacity;
    private readonly Dictionary<int, int> mapping = new();
    private T[] itemStorage = Array.Empty<T>();
    private readonly Queue<int> emptySlots = new();

    public int Count => itemStorage.Length - emptySlots.Count;

    public Pool(int maximumCapacity = int.MaxValue)
    {
      this.maximumCapacity = maximumCapacity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool HasCapacity()
    {
      return maximumCapacity - itemStorage.Length > 0;
    }
    public ref T Store(int id, in T item)
    {
      bool mappingExists = mapping.TryGetValue(id, out int itemId);

      if (!mappingExists)
      {
        itemId = CreateMapping(id, in item);
      }

      return ref itemStorage[itemId];
    }


    public ref T Receive(int id)
    {
      bool mappingExists = mapping.TryGetValue(id, out int itemId);

      if (!mappingExists)
        throw new MappingNotFoundException(id);

      return ref itemStorage[itemId];
    }

    public void Remove(int id)
    {
      bool mappingExists = mapping.TryGetValue(id, out int itemId);

      if (!mappingExists)
        throw new MappingNotFoundException(id);

      emptySlots.Enqueue(itemId);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int CreateMapping(int id, in T item)
    {
      if (emptySlots.Count == 0)
      {
        IncreaseStorage();
      }

      int slot = emptySlots.Dequeue();

      itemStorage[slot] = item;
      mapping.Add(id, slot);
      return slot;
    }

    private void IncreaseStorage()
    {
      int currentSize = itemStorage.Length;

      if (currentSize >= maximumCapacity)
        throw new MaximumCapacityReachedException(maximumCapacity);

      int newSize = Math.Min(currentSize * 2, maximumCapacity);
      newSize = Math.Max(InitialSize, newSize);
      Array.Resize(ref itemStorage, newSize);
      for (int i = currentSize; i < newSize; i++)
        emptySlots.Enqueue(i);
    }
  }
}

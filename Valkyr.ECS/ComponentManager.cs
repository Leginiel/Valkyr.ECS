using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Valkyr.ECS
{
  internal static class ComponentManager
  {
    private static readonly ConcurrentDictionary<Type, IComponentManager> componentManagers = new();
    public static ComponentManager<T> Instance<T>()
      where T : IComponent
    {
      return (ComponentManager<T>)componentManagers.GetOrAdd(typeof(T), new ComponentManager<T>());
    }

    public static void Remove(short worldId)
    {
      foreach (KeyValuePair<Type, IComponentManager> entry in componentManagers)
      {
        entry.Value.Remove(worldId);
      }
    }
  }
  internal class ComponentManager<T> : IComponentManager
      where T : IComponent
  {
    private readonly ConcurrentDictionary<short, IComponentPool<T>> pools = new();

    public int PoolCount => pools.Count;

    public IComponentPool<T> GetOrCreate(short worldId, int maxCapacity)
    {
      return pools.GetOrAdd(worldId, new ComponentPool<T>(maxCapacity));
    }
    public bool Remove(short worldId)
    {
      return pools.TryRemove(worldId, out _);
    }
  }
}

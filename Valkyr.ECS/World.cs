using System;
using System.Collections.Generic;

namespace Valkyr.ECS
{
  public sealed partial class World : IWorld, IDisposable
  {
    private const int InitialSize = 4;
    private readonly int maxCapacity;
    private readonly Queue<int> freeEntities = new();
    private Entity[] entities = Array.Empty<Entity>();
    private bool disposedValue;

    public short Id { get; }

    public World(short id, int maxCapacity = int.MaxValue)
    {
      Id = id;
      this.maxCapacity = maxCapacity;
    }
    public ref Entity CreateEntity()
    {
      if (freeEntities.Count == 0)
      {
        IncreaseStorageSize();
      }
      int nextId = freeEntities.Dequeue();

      return ref entities[nextId];
    }

    public bool Has<T>(int entityId)
      where T : IComponent
    {
      return ComponentManager.Instance<T>().GetOrCreate(Id, maxCapacity).Has(entityId);
    }

    public ref T Get<T>(int entityId)
      where T : IComponent
    {
      return ref ComponentManager.Instance<T>().GetOrCreate(Id, maxCapacity).Receive(entityId);
    }

    public void Set<T>(int entityId, in T component)
      where T : IComponent
    {
      ComponentManager.Instance<T>().GetOrCreate(Id, maxCapacity).Store(entityId, in component);
    }

    public void Remove<T>(int entityId)
      where T : IComponent
    {
      ComponentManager.Instance<T>().GetOrCreate(Id, maxCapacity).Remove(entityId);
    }
    public void Dispose()
    {
      Dispose(disposing: true);
      GC.SuppressFinalize(this);
    }
    public void IterateEntities(ActionRef<Entity> entityCallback, IFilterExpression filter)
    {
      for (int i = 0; i < entities.Length; i++)
      {
        ref Entity entity = ref entities[i];
        if (!freeEntities.Contains(i) && filter.Matches(ref entity))
        {
          entityCallback.Invoke(ref entity);
        }
      }
    }

    private void IncreaseStorageSize()
    {
      int currentSize = entities.Length;

      if (currentSize >= maxCapacity)
        throw new MaximumCapacityReachedException(maxCapacity);

      int newSize = Math.Min(currentSize * 2, maxCapacity);
      newSize = Math.Max(InitialSize, newSize);
      Array.Resize(ref entities, newSize);
      for (int i = currentSize; i < newSize; i++)
      {
        freeEntities.Enqueue(i);
        entities[i] = new(i, this);
      }
    }
    private void Dispose(bool disposing)
    {
      if (!disposedValue)
      {
        if (disposing)
        {
          entities = null;
          ComponentManager.Remove(Id);
        }
        disposedValue = true;
      }
    }

  }
}

using System;
using System.Collections.Generic;

namespace Valkyr.ECS
{
  public sealed class Context : IDisposable
  {
    private bool disposedValue;
    private readonly Dictionary<short, IWorld> worlds = new();
    private readonly Dictionary<Type, ISystem> systems = new();
    private short nextWorldId = 0;

    public IWorld CreateWorld(int maxCapacity = int.MaxValue)
    {
      IWorld result = new World(nextWorldId++, maxCapacity);
      worlds.Add(result.Id, result);

      return result;
    }
    public void DestroyWorld(IWorld world)
    {
      DestroyWorld(world.Id);
    }
    public void DestroyWorld(short id)
    {
      IWorld world = worlds[id];
      worlds.Remove(id);
      world.Dispose();
    }
    public bool HasWorld(short id)
    {
      return worlds.ContainsKey(id);
    }
    public void RegisterSystem<T>()
      where T : ISystem, new()
    {
      ISystem system = new T();

      systems.Add(system.GetType(), system);
    }
    public void UnregisterSystem<T>()
      where T : ISystem
    {
      Type systemType = typeof(T);

      EnsureSystemRegistered(systemType);

      systems.Remove(systemType);
    }
    public bool IsRegistered<T>()
      where T : ISystem
    {
      return systems.ContainsKey(typeof(T));
    }
    public bool IsEnabled<T>()
      where T : ISystem
    {
      return EnsureSystemRegistered(typeof(T)).Enabled;
    }

    public void EnableSystem<T>()
      where T : ISystem
    {
      EnsureSystemRegistered(typeof(T)).Enabled = true;
    }
    public void DisableSystem<T>()
      where T : ISystem
    {
      EnsureSystemRegistered(typeof(T)).Enabled = false;
    }
    public void Update()
    {
      foreach (IWorld world in worlds.Values)
      {
        if (world.Active)
        {
          Update(world);
        }
      }
    }

    private void Update(IWorld world)
    {
      foreach (ISystem system in systems.Values)
      {
        system.Update(world);
      }
    }

    public void Dispose()
    {
      // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
      Dispose(disposing: true);
      GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
      if (!disposedValue)
      {
        if (disposing)
        {
          foreach (KeyValuePair<short, IWorld> entry in worlds)
          {
            entry.Value.Dispose();
          }

          systems.Clear();
          worlds.Clear();
        }
        disposedValue = true;
      }
    }
    private ISystem EnsureSystemRegistered(Type systemType)
    {
      if (!systems.ContainsKey(systemType))
        throw new InvalidOperationException($"System {systemType.Name} is not registered.");

      return systems[systemType];
    }

  }
}

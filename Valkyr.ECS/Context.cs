using System;
using System.Collections.Generic;

namespace Valkyr.ECS
{
  public sealed class Context<TState> : IDisposable
  {
    private bool disposedValue;
    private readonly Dictionary<short, IWorld> worlds = new();
    private IRunnable<TState> runner;
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
      where T : IRunnable<TState>, new()
    {
      if (runner is RunnableContainer<TState> container)
      {
        container.Add(new T());
        runner = container;

      }
      else if (runner is not null)
      {
        container = new RunnableContainer<TState>();

        container.Add(runner);
        container.Add(new T());
        runner = container;
      }
      else
      {
        runner = new T();
      }
    }
    public void UnregisterSystem<T>()
      where T : class, IRunnable<TState>
    {
      T system = EnsureSystemRegistered<T>();

      if (runner is RunnableContainer<TState> container)
      {
        container.Remove(system);
      }
      else
      {
        runner = null;
      }
    }
    public bool IsRegistered<T>()
      where T : class, IRunnable<TState>
    {
      return runner is not null && runner.Supports<T>(out _);
    }
    public bool IsEnabled<T>()
      where T : class, IRunnable<TState>
    {
      return EnsureSystemRegistered<T>().Enabled;
    }

    public void EnableSystem<T>()
      where T : class, IRunnable<TState>
    {
      EnsureSystemRegistered<T>().Enabled = true;
    }
    public void DisableSystem<T>()
      where T : class, IRunnable<TState>
    {
      EnsureSystemRegistered<T>().Enabled = false;
    }
    public void Update(TState state)
    {
      foreach (IWorld world in worlds.Values)
      {
        if (world.Active)
        {
          Update(world, state);
        }
      }
    }

    private void Update(IWorld world, TState state)
    {
      world.IterateEntities((entity) => runner.Run(entity, state), FilterExpressions.All());
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

          runner = null;
          worlds.Clear();
        }
        disposedValue = true;
      }
    }
    private T EnsureSystemRegistered<T>()
      where T : class, IRunnable<TState>
    {
      if (runner is null || !runner.Supports<T>(out IRunnable<TState> result))
        throw new InvalidOperationException($"System {typeof(T).Name} is not registered.");

      return result as T;
    }

  }
}

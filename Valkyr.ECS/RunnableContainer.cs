using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Valkyr.ECS
{
  public class RunnableContainer<TState> : IRunnable<TState>
  {
    private readonly IList<IRunnable<TState>> internalContainer;
    private readonly List<Task> tasks = new();

    public bool RunInParallel { get; }

    public RunnableContainer(bool runInParallel = false)
      : this(new List<IRunnable<TState>>(), runInParallel) { }
    internal RunnableContainer(IList<IRunnable<TState>> internalContainer, bool runInParallel = false)
    {
      this.internalContainer = internalContainer;
      RunInParallel = runInParallel;
    }
    public bool CanProcess(Entity entity)
    {
      return true;
    }
    public void Add(IRunnable<TState> runnable)
    {
      if (runnable is null)
        throw new ArgumentNullException(nameof(runnable));

      internalContainer.Add(runnable);
    }
    public void Remove(IRunnable<TState> runnable)
    {
      if (runnable is null)
        throw new ArgumentNullException(nameof(runnable));

      if (!internalContainer.Contains(runnable))
        throw new ArgumentException("Container doesn't contain runnable", nameof(runnable));

      internalContainer.Remove(runnable);
    }
    public async Task Run(Entity entity, TState state)
    {
      if (RunInParallel)
        await RunParallel(entity, state);
      else
        await RunSequential(entity, state);
    }

    private async Task RunSequential(Entity entity, TState state)
    {
      foreach (IRunnable<TState> runnable in internalContainer)
      {
        if (runnable.CanProcess(entity))
          await runnable.Run(entity, state);
      }
    }

    private async Task RunParallel(Entity entity, TState state)
    {
      foreach (IRunnable<TState> runnable in internalContainer)
      {
        if (runnable.CanProcess(entity))
          tasks.Add(runnable.Run(entity, state));
      }
      await Task.WhenAll(tasks);
      tasks.Clear();
    }
  }
}

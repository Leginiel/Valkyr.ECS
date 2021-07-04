using System.Threading.Tasks;

namespace Valkyr.ECS
{
  public abstract class System<T, TState> : IRunnable<TState>
    where T : struct, IComponent
  {
    public bool Enabled { get; set; } = true;

    public bool CanProcess(Entity entity)
    {
      return Enabled && FilterExpressions.Component<T>().Matches(entity);
    }

    public abstract Task Run(Entity entity, TState state);

    public bool Supports<T1>(out IRunnable<TState> outRunnable)
      where T1 : IRunnable<TState>
    {
      bool result = GetType().Equals(typeof(T1));
      outRunnable = (result) ? this : default;
      return result;
    }
  }
}

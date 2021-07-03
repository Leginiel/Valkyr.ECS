using System.Threading.Tasks;

namespace Valkyr.ECS
{
  public abstract class System<T, TState> : IRunnable<TState>
    where T : struct, IComponent
  {
    public bool Enabled { get; set; }

    public bool CanProcess(Entity entity)
    {
      return Enabled && FilterExpressions.Component<T>().Matches(ref entity);
    }

    public abstract Task Run(Entity entity, TState state);
  }
}

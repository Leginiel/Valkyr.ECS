using System.Threading.Tasks;

namespace Valkyr.ECS
{
  public interface IRunnable<TState>
  {
    bool CanProcess(Entity entity);
    Task Run(Entity entity, TState state);
  }
}

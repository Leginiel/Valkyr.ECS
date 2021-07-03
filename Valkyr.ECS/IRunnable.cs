using System.Threading.Tasks;

namespace Valkyr.ECS
{
  public interface IRunnable<TState>
  {
    bool Enabled { get; set; }
    bool CanProcess(Entity entity);
    Task Run(Entity entity, TState state);
  }
}

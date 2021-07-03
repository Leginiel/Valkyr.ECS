using System.Threading.Tasks;

namespace Valkyr.ECS.Tests
{
  public class UnittestSystem : System<UnittestComponent, int>
  {
    public override Task Run(Entity entity, int state)
    {
      return Task.CompletedTask;
    }
  }
}

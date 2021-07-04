using System.Collections.Generic;
using System.Threading.Tasks;

namespace Valkyr.ECS.Tests
{
  public class UnittestSystem : System<UnittestComponent, int>
  {
    public static List<Entity> UpdatedEntities { get; } = new();

    public override Task Run(Entity entity, int state)
    {
      UpdatedEntities.Add(entity);
      return Task.CompletedTask;
    }
  }
  public class UnittestSystem2 : System<UnittestComponent2, int>
  {
    public override Task Run(Entity entity, int state)
    {
      return Task.CompletedTask;
    }
  }
}

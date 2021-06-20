using System.Collections.Generic;

namespace Valkyr.ECS.Tests
{
  public class UnittestSystem : System<UnittestComponent>
  {
    public static readonly List<IWorld> UpdatedWorlds = new();
    protected override void Update(ref Entity entity)
    {
    }
    public override void Update(IWorld world)
    {
      UpdatedWorlds.Add(world);
    }

    internal static void Reset()
    {
      UpdatedWorlds.Clear();
    }
  }
}

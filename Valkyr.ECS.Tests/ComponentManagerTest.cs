using FluentAssertions;
using Xunit;

namespace Valkyr.ECS.Tests
{
  public class ComponentManagerTest
  {
    [Fact]
    public void GetOrCreate_ValidWorldIdAndMaxCapacity_ValidPoolReturned()
    {
      short worldId = 1;
      int maxCapacity = 1;
      ComponentManager<UnittestComponent> componentManager = ComponentManager.Instance<UnittestComponent>();

      componentManager.GetOrCreate(worldId, maxCapacity).Should().NotBeNull();
    }
    [Fact]
    public void Remove_ValidWorldIdAndPool_PoolRemoved()
    {
      short worldId = 1;
      int maxCapacity = 1;
      ComponentManager<UnittestComponent> componentManager = ComponentManager.Instance<UnittestComponent>();

      componentManager.GetOrCreate(worldId, maxCapacity).Should().NotBeNull();
      componentManager.Remove(worldId).Should().BeTrue();
    }
  }
}

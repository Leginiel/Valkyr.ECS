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

      ComponentManager<UnittestComponent>.GetOrCreate(worldId, maxCapacity).Should().NotBeNull();
    }
  }
}

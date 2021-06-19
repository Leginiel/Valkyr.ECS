using FluentAssertions;
using Xunit;

namespace Valkyr.ECS.Tests
{
  public class FilterExpressionsTest
  {
    [Fact]
    public void All_AnyValue_ReturnsTrue()
    {
      Entity[] entities = new Entity[] {
        new Entity(0, null),
        new Entity(1, null)
      };
      FilterExpressions.All.Matches(ref entities[0]).Should().BeTrue();
      FilterExpressions.All.Matches(ref entities[1]).Should().BeTrue();
    }
  }
}

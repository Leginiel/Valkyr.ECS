using FluentAssertions;
using System;
using Xunit;

namespace Valkyr.ECS.Tests
{
  public class WorldTest
  {
    [Fact]
    public void CreateEntity_Successful_ValidEntityId()
    {
      World world = new();

      world.CreateEntity().Id.Should().Be(0);
    }
    [Fact]
    public void CreateEntity_MaximumCapacityReached_ThrowEntityException()
    {
      World world = new(0);
      Action action = () => world.CreateEntity();
      action.Should().Throw<EntityException>();
    }
    [Fact]
    public void CreateEntity_RequestMultipleEntities_UniqueEntities()
    {
      World world = new();

      world.CreateEntity().Should().NotBeSameAs(world.CreateEntity());
    }
  }
}

using FluentAssertions;
using Moq;
using System;
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
      FilterExpressions.All().Matches(entities[0]).Should().BeTrue();
      FilterExpressions.All().Matches(entities[1]).Should().BeTrue();
    }
    [Fact]
    public void Component_EntitiesHaveComponent_ReturnsTrue()
    {
      Mock<IWorld> worldMock = new();
      Entity[] entities = new Entity[] {
        new Entity(0, worldMock.Object),
        new Entity(1, worldMock.Object)
      };

      worldMock.Setup(_ => _.Has<UnittestComponent>(It.IsAny<int>())).Returns(true);

      FilterExpressions.Component<UnittestComponent>().Matches(entities[0]).Should().BeTrue();
      FilterExpressions.Component<UnittestComponent>().Matches(entities[1]).Should().BeTrue();
    }
    [Fact]
    public void Component_ParamNotAnEntity_ThrowsArgumentExeption()
    {
      int[] valuesEntities = new int[] { 1 };

      Action action = () => FilterExpressions.Component<UnittestComponent>().Matches(valuesEntities[0]);

      action.Should().Throw<ArgumentException>();
    }
    [Fact]
    public void Component_EntitiesHaveNotComponent_ReturnsFalse()
    {
      Mock<IWorld> worldMock = new();
      Entity[] entities = new Entity[] {
        new Entity(0, worldMock.Object),
        new Entity(1, worldMock.Object)
      };

      worldMock.Setup(_ => _.Has<UnittestComponent>(It.IsAny<int>())).Returns(false);
      FilterExpressions.Component<UnittestComponent>().Matches(entities[0]).Should().BeFalse();
      FilterExpressions.Component<UnittestComponent>().Matches(entities[1]).Should().BeFalse();
    }
    [Fact]
    public void Component_MixedEntitiesHaveComponents_ReturnsFalse()
    {
      Mock<IWorld> worldMock = new();
      Entity[] entities = new Entity[] {
        new Entity(0, worldMock.Object),
        new Entity(1, worldMock.Object)
      };

      worldMock.SetupSequence(_ => _.Has<UnittestComponent>(It.IsAny<int>()))
               .Returns(false)
               .Returns(true);
      FilterExpressions.Component<UnittestComponent>().Matches(entities[0]).Should().BeFalse();
      FilterExpressions.Component<UnittestComponent>().Matches(entities[1]).Should().BeTrue();
    }
  }
}

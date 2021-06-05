using FluentAssertions;
using Moq;
using Xunit;

namespace Valkyr.ECS.Tests
{
  public class EntityTest
  {
    [Fact]
    public void Equals_ComparisonWithSameEntity_True()
    {
      Mock<IWorld> worldMock = new();
      Entity entity = new(1, worldMock.Object);

      entity.Equals(entity).Should().BeTrue();
    }
    [Fact]
    public void Equals_ComparisonWithDifferentEntity_False()
    {
      Mock<IWorld> worldMock = new();
      Entity entity = new(1, worldMock.Object);
      Entity entity2 = new(2, worldMock.Object);

      entity.Equals(entity2).Should().BeFalse();
    }
    [Fact]
    public void Equals_ComparisonWithNull_False()
    {
      Mock<IWorld> worldMock = new();
      Entity entity = new(1, worldMock.Object);

      entity.Equals(null).Should().BeFalse();
    }
    [Fact]
    public void GetHashCode_SameEntityId_SameHashcode()
    {
      Mock<IWorld> worldMock = new();
      Entity entity = new(1, worldMock.Object);
      Entity entity2 = new(1, worldMock.Object);

      entity.GetHashCode().Should().Be(entity2.GetHashCode());
    }
    [Fact]
    public void Has_EntitiesHasComponent_True()
    {
      Mock<IWorld> worldMock = new();
      Entity entity = new(0, worldMock.Object);

      worldMock.Setup(_ => _.Has<It.IsAnyType>(It.IsAny<int>())).Returns(true);

      entity.Has<UnittestComponent>().Should().BeTrue();
    }
    [Fact]
    public void Has_EntityNotHavingComponents_False()
    {
      Mock<IWorld> worldMock = new();
      Entity entity = new(0, worldMock.Object);

      worldMock.Setup(_ => _.Has<It.IsAnyType>(It.IsAny<int>())).Returns(false);

      entity.Has<UnittestComponent>().Should().BeFalse();
    }
    [Fact]
    public void Get_EntityNotHavingComponent_DefaultValue()
    {
      World world = new();
      Entity entity = new(0, world);

      entity.Get<UnittestComponent>().Should().Be(default);
    }
    [Fact]
    public void Get_EntityHavingComponent_ComponentIsReturned()
    {
      World world = new();
      Entity entity = new(0, world);
      UnittestComponent component = new(1);

      entity.Set(component);
      entity.Get<UnittestComponent>().Should().Be(component);
    }
    [Fact]
    public void Set_EntityNotHavingComponent_ComponentIsSet()
    {
      World world = new();
      Entity entity = new(0, world);
      UnittestComponent component = new(1);

      entity.Set(component);
      entity.Has<UnittestComponent>().Should().BeTrue();
      entity.Get<UnittestComponent>().Should().Be(component);
    }

    [Fact]
    public void Set_EntityHavingComponent_ComponentIsOverwritten()
    {
      World world = new();
      Entity entity = new(0, world);
      UnittestComponent component = new(1);
      UnittestComponent component2 = new(2);

      entity.Set(component);
      entity.Set(component2);
      entity.Has<UnittestComponent>().Should().BeTrue();
      entity.Get<UnittestComponent>().Should().Be(component2);
    }

  }
}

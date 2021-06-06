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
    public void Has_None_WorldIsCalledWithOwnId()
    {
      Mock<IWorld> worldMock = new();
      Entity entity = new(1, worldMock.Object);

      worldMock.Setup(_ => _.Has<UnittestComponent>(entity.Id)).Verifiable();

      entity.Has<UnittestComponent>();

      worldMock.VerifyAll();
    }
    [Fact]
    public void Get_None_WorldIsCalledWithOwnId()
    {
      Mock<IWorld> worldMock = new();
      Entity entity = new(1, worldMock.Object);

      worldMock.Setup(_ => _.Get<UnittestComponent>(entity.Id)).Verifiable();

      entity.Get<UnittestComponent>();

      worldMock.VerifyAll();
    }
    [Fact(Skip = "https://github.com/castleproject/Core/issues/430#issuecomment-756309040")]
    public void Set_None_WorldIsCalledWithOwnId()
    {
      Mock<IWorld> worldMock = new();
      Entity entity = new(1, worldMock.Object);
      UnittestComponent component = new(1);

      worldMock.Setup(_ => _.Set(entity.Id, in component)).Verifiable();

      entity.Set(component);

      worldMock.VerifyAll();
    }
    [Fact]
    public void Remove_None_WorldIsCalledWithOwnId()
    {
      Mock<IWorld> worldMock = new();
      Entity entity = new(1, worldMock.Object);

      worldMock.Setup(_ => _.Remove<UnittestComponent>(entity.Id)).Verifiable();

      entity.Remove<UnittestComponent>();

      worldMock.VerifyAll();
    }
  }
}

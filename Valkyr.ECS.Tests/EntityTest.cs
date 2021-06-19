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
    public void EqualsOperator_ComparisonWithSameEntity_True()
    {
      Mock<IWorld> worldMock = new();
      Entity entity = new(1, worldMock.Object);

#pragma warning disable CS1718 // Comparison made to same variable
      (entity == entity).Should().BeTrue();
#pragma warning restore CS1718 // Comparison made to same variable
    }
    [Fact]
    public void EqualsOperator_ComparisonWithDifferentEntity_False()
    {
      Mock<IWorld> worldMock = new();
      Entity entity = new(1, worldMock.Object);
      Entity entity2 = new(2, worldMock.Object);

      (entity == entity2).Should().BeFalse();
    }
    [Fact]
    public void NotEqualsOperator_ComparisonWithSameEntity_False()
    {
      Mock<IWorld> worldMock = new();
      Entity entity = new(1, worldMock.Object);

#pragma warning disable CS1718 // Comparison made to same variable
      (entity != entity).Should().BeFalse();
#pragma warning restore CS1718 // Comparison made to same variable
    }
    [Fact]
    public void NotEqualsOperator_ComparisonWithDifferentEntity_True()
    {
      Mock<IWorld> worldMock = new();
      Entity entity = new(1, worldMock.Object);
      Entity entity2 = new(2, worldMock.Object);

      (entity != entity2).Should().BeTrue();
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
    [Fact(Skip = "Waiting for Moq 5 return ref support")]
    public void Get_None_WorldIsCalledWithOwnId()
    {
      Mock<IWorld> worldMock = new();
      Entity entity = new(1, worldMock.Object);

      //worldMock.Verify(r_ => _.Get<UnittestComponent>(entity.Id));

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

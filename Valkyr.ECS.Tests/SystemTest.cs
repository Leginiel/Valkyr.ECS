using FluentAssertions;
using Moq;
using Xunit;

namespace Valkyr.ECS.Tests
{
  public class SystemTest
  {
    [Fact]
    public void CanProcess_ValidEntityAndSystemIsEnabled_True()
    {
      Mock<IWorld> worldMock = new();
      Mock<System<UnittestComponent, int>> systemMock = new()
      {
        CallBase = true
      };
      Entity entity = new(1, worldMock.Object);
      worldMock.Setup(_ => _.Has<UnittestComponent>(It.IsAny<int>())).Returns(true);

      systemMock.Object.CanProcess(entity).Should().BeTrue();
    }
    [Fact]
    public void CanProcess_ValidEntityAndSystemIsDisabled_False()
    {
      Mock<IWorld> worldMock = new();
      Mock<System<UnittestComponent, int>> systemMock = new()
      {
        CallBase = true
      };
      Entity entity = new(1, worldMock.Object);

      worldMock.Setup(_ => _.Has<UnittestComponent>(It.IsAny<int>())).Returns(true);

      systemMock.Object.Enabled = false;
      systemMock.Object.CanProcess(entity).Should().BeFalse();
    }
    [Fact]
    public void CanProcess_InvalidEntityAndSystemIsEnabled_False()
    {
      Mock<IWorld> worldMock = new();
      Mock<System<UnittestComponent, int>> systemMock = new()
      {
        CallBase = true
      };
      Entity entity = new(1, worldMock.Object);

      worldMock.Setup(_ => _.Has<UnittestComponent>(It.IsAny<int>())).Returns(false);

      systemMock.Object.CanProcess(entity).Should().BeFalse();
    }
    [Fact]
    public void CanProcess_InvalidEntityAndSystemIsDisabled_False()
    {
      Mock<IWorld> worldMock = new();
      Mock<System<UnittestComponent, int>> systemMock = new()
      {
        CallBase = true
      };
      Entity entity = new(1, worldMock.Object);

      worldMock.Setup(_ => _.Has<UnittestComponent>(It.IsAny<int>())).Returns(false);

      systemMock.Object.Enabled = false;
      systemMock.Object.CanProcess(entity).Should().BeFalse();
    }
  }
}

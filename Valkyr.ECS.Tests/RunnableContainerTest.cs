using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Valkyr.ECS.Tests
{
  public class RunnableContainerTest
  {
    [Fact]
    public void Add_ValidRunnable_SuccessfullyAddedToContainer()
    {
      List<IRunnable<int>> internalContainer = new();
      RunnableContainer<int> container = new(internalContainer);
      Mock<IRunnable<int>> runnableMock = new();

      container.Add(runnableMock.Object);

      internalContainer.Should().Contain(runnableMock.Object);
    }
    [Fact]
    public void Add_Null_ThrowsArgumentNullException()
    {
      List<IRunnable<int>> internalContainer = new();
      RunnableContainer<int> container = new();
      Action action = () => container.Add(null);

      action.Should().Throw<ArgumentNullException>();
    }
    [Fact]
    public void Remove_ValidAddedRunnable_SuccessfullyRemovedFromContainer()
    {
      Mock<IRunnable<int>> runnableMock = new();
      List<IRunnable<int>> internalContainer = new() { runnableMock.Object };
      RunnableContainer<int> container = new(internalContainer);

      container.Remove(runnableMock.Object);

      internalContainer.Should().NotContain(runnableMock.Object);
    }
    [Fact]
    public void Remove_ValidNotAddedRunnable_ThrowsArgumentException()
    {
      Mock<IRunnable<int>> runnableMock = new();
      List<IRunnable<int>> internalContainer = new();
      RunnableContainer<int> container = new();
      Action action = () => container.Remove(runnableMock.Object);

      action.Should().Throw<ArgumentException>();
    }
    [Fact]
    public void Remove_Null_ThrowsArgumentNullException()
    {
      List<IRunnable<int>> internalContainer = new();
      RunnableContainer<int> container = new();
      Action action = () => container.Remove(null);

      action.Should().Throw<ArgumentNullException>();
    }
    [Fact]
    public async Task Run_ValidEntityAndStateAndParallelExecution_AllEntriesUpdated()
    {
      Mock<IRunnable<int>> runnableMock = new();
      Mock<IRunnable<int>> runnableMock2 = new();
      Entity entity = new(0, null);
      RunnableContainer<int> container = new(true);


      runnableMock.Setup(_ => _.CanProcess(It.IsAny<Entity>())).Returns(true);
      runnableMock2.Setup(_ => _.CanProcess(It.IsAny<Entity>())).Returns(true);

      container.Add(runnableMock.Object);
      container.Add(runnableMock2.Object);
      await container.Run(entity, 1);

      runnableMock.Verify(_ => _.Run(It.IsAny<Entity>(), 1), Times.Once());
      runnableMock2.Verify(_ => _.Run(It.IsAny<Entity>(), 1), Times.Once());
    }
    [Fact]
    public async Task Run_ValidEntityAndState_AllEntriesUpdated()
    {
      Mock<IRunnable<int>> runnableMock = new();
      Mock<IRunnable<int>> runnableMock2 = new();
      Entity entity = new(0, null);
      RunnableContainer<int> container = new();


      runnableMock.Setup(_ => _.CanProcess(It.IsAny<Entity>())).Returns(true);
      runnableMock2.Setup(_ => _.CanProcess(It.IsAny<Entity>())).Returns(true);

      container.Add(runnableMock.Object);
      container.Add(runnableMock2.Object);

      await container.Run(entity, 1);

      runnableMock.Verify(_ => _.Run(It.IsAny<Entity>(), 1), Times.Once());
      runnableMock2.Verify(_ => _.Run(It.IsAny<Entity>(), 1), Times.Once());
    }
    [Fact]
    public async Task Run_NotProcessableEntityAndState_AllowedEntriesUpdated()
    {
      Mock<IRunnable<int>> runnableMock = new();
      Mock<IRunnable<int>> runnableMock2 = new();
      Entity entity = new(0, null);
      RunnableContainer<int> container = new();

      runnableMock.Setup(_ => _.CanProcess(It.IsAny<Entity>())).Returns(false);
      runnableMock2.Setup(_ => _.CanProcess(It.IsAny<Entity>())).Returns(true);

      container.Add(runnableMock.Object);
      container.Add(runnableMock2.Object);

      await container.Run(entity, 1);

      runnableMock.Verify(_ => _.Run(It.IsAny<Entity>(), 1), Times.Never());
      runnableMock2.Verify(_ => _.Run(It.IsAny<Entity>(), 1), Times.Once());
    }
    [Fact]
    public async Task Run_ContainerIsDisabled_NothingUpdated()
    {
      Mock<IRunnable<int>> runnableMock = new();
      Mock<IRunnable<int>> runnableMock2 = new();
      Entity entity = new(0, null);
      RunnableContainer<int> container = new()
      {
        Enabled = false
      };

      container.Add(runnableMock.Object);
      container.Add(runnableMock2.Object);

      await container.Run(entity, 1);

      runnableMock.Verify(_ => _.Run(It.IsAny<Entity>(), 1), Times.Never());
      runnableMock2.Verify(_ => _.Run(It.IsAny<Entity>(), 1), Times.Never());
    }
    [Fact]
    public void Supports_ContainsValidSystemType_True()
    {
      Mock<IRunnable<int>> runnableMock = new();
      Mock<IRunnable<int>> runnableMock2 = new();
      Entity entity = new(0, null);
      RunnableContainer<int> container = new()
      {
        Enabled = false
      };

      runnableMock.Setup(_ => _.Supports<UnittestSystem>(out It.Ref<IRunnable<int>>.IsAny)).Returns(true);
      runnableMock2.Setup(_ => _.Supports<UnittestSystem>(out It.Ref<IRunnable<int>>.IsAny)).Returns(false);

      container.Add(runnableMock.Object);
      container.Add(runnableMock2.Object);

      container.Supports<UnittestSystem>(out _).Should().BeTrue();
    }
    [Fact]
    public void Supports_DoesNotContainSystemType_False()
    {
      Mock<IRunnable<int>> runnableMock = new();
      Mock<IRunnable<int>> runnableMock2 = new();
      IRunnable<int> _;
      Entity entity = new(0, null);
      RunnableContainer<int> container = new()
      {
        Enabled = false
      };

      runnableMock.Setup(_ => _.Supports<UnittestSystem>(out It.Ref<IRunnable<int>>.IsAny)).Returns(false);
      runnableMock2.Setup(_ => _.Supports<UnittestSystem>(out It.Ref<IRunnable<int>>.IsAny)).Returns(false);

      container.Add(runnableMock.Object);
      container.Add(runnableMock2.Object);

      container.Supports<UnittestSystem>(out _).Should().BeFalse();
    }
  }
}

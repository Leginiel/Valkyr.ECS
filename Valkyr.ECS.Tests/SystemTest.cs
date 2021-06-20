using Moq;
using Moq.Protected;
using Xunit;

namespace Valkyr.ECS.Tests
{
  public class SystemTest
  {
    // Helper interface to make the MockSetup easier:
    // see example in https://github.com/moq/moq4/issues/223
    private interface ISystemImpl
    {
      public void Update(ref Entity entity);
    }
    [Fact]
    public void Update_ValidWorldAndThereAreEnities_UpdateMethodIsCalledForEveryValidEntity()
    {
      Mock<IWorld> worldMock = new();
      Mock<System<UnittestComponent>> systemMock = new()
      {
        CallBase = true
      };
      Entity[] entities = new Entity[] {
          new Entity(1, worldMock.Object),
          new Entity(2, worldMock.Object),
          new Entity(3, worldMock.Object),
          new Entity(4, worldMock.Object)
        };

      worldMock.Setup(_ => _.IterateEntities(It.IsAny<ActionRef<Entity>>(), It.IsAny<IFilterExpression>()))
               .Callback<ActionRef<Entity>, IFilterExpression>((callback, filter) =>
               {
                 for (int i = 0; i < entities.Length; i++)
                 {
                   callback.Invoke(ref entities[i]);
                 }
               });
      worldMock.SetupSequence(_ => _.Has<UnittestComponent>(It.IsAny<int>()))
               .Returns(true)
               .Returns(false)
               .Returns(true)
               .Returns(true);

      systemMock.Object.Update(worldMock.Object);
      systemMock.Protected()
                .As<ISystemImpl>()
                .Verify(_ => _.Update(ref entities[0]), Times.Once());
      systemMock.Protected()
                .As<ISystemImpl>()
                .Verify(_ => _.Update(ref entities[2]), Times.Once());
      systemMock.Protected()
                .As<ISystemImpl>()
                .Verify(_ => _.Update(ref entities[3]), Times.Once());
    }
    [Fact]
    public void Update_ValidWorldAndThereAreEnitiesDisabledSystem_UpdateMethodIsNotCalled()
    {
      Mock<IWorld> worldMock = new();
      Mock<System<UnittestComponent>> systemMock = new()
      {
        CallBase = true
      };
      Entity[] entities = new Entity[] {
          new Entity(1, worldMock.Object),
          new Entity(2, worldMock.Object),
          new Entity(3, worldMock.Object),
          new Entity(4, worldMock.Object)
        };

      worldMock.Setup(_ => _.IterateEntities(It.IsAny<ActionRef<Entity>>(), It.IsAny<IFilterExpression>()))
               .Callback<ActionRef<Entity>, IFilterExpression>((callback, filter) =>
               {
                 for (int i = 0; i < entities.Length; i++)
                 {
                   callback.Invoke(ref entities[i]);
                 }
               });
      worldMock.SetupSequence(_ => _.Has<UnittestComponent>(It.IsAny<int>()))
               .Returns(true)
               .Returns(false)
               .Returns(true)
               .Returns(true);
      systemMock.Protected()
                .As<ISystemImpl>()
                .Verify(_ => _.Update(ref It.Ref<Entity>.IsAny), Times.Never());

      systemMock.Object.Enabled = false;

      systemMock.Object.Update(worldMock.Object);

      systemMock.VerifyAll();
    }
  }
}

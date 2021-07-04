using Moq;
using Xunit;

namespace Valkyr.ECS.Tests
{
  public class EntityTest
  {
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

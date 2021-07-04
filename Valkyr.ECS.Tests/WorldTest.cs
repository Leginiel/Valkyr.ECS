using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Valkyr.ECS.Tests
{
  public class WorldTest
  {
    [Fact]
    public void CreateEntity_Successful_ValidEntityId()
    {
      using World world = new(0);

      world.CreateEntity().Id.Should().Be(0);
    }
    [Fact]
    public void CreateEntity_MaximumCapacityReached_ThrowEntityException()
    {
      using World world = new(0, 0);
      Action action = () => world.CreateEntity();
      action.Should().Throw<MaximumCapacityReachedException>();
    }
    [Fact]
    public void CreateEntity_RequestMultipleEntities_UniqueEntities()
    {
      using World world = new(0);

      world.CreateEntity().Should().NotBeSameAs(world.CreateEntity());
    }
    [Fact]
    public void Has_EntitiesHasComponent_True()
    {
      using World world = new(0);
      Entity entity = world.CreateEntity();
      UnittestComponent component = new(1);

      world.Set(entity.Id, component);
      world.Has<UnittestComponent>(entity.Id).Should().BeTrue();
    }
    [Fact]
    public void Has_EntityNotHavingComponents_False()
    {
      using World world = new(0);
      Entity entity = world.CreateEntity();

      world.Has<UnittestComponent>(entity.Id).Should().BeFalse();
    }
    [Fact]
    public void Get_EntityNotHavingComponent_DefaultValue()
    {
      using World world = new(0);
      Entity entity = world.CreateEntity();
      Action action = () => world.Get<UnittestComponent>(world.Id);

      action.Should().Throw<MappingNotFoundException>();
    }
    [Fact]
    public void Get_EntityHavingComponent_ComponentIsReturned()
    {
      using World world = new(0);
      Entity entity = world.CreateEntity();
      UnittestComponent component = new(1);

      world.Set(entity.Id, component);
      world.Get<UnittestComponent>(entity.Id).Should().Be(component);
    }
    [Fact]
    public void Set_EntityNotHavingComponent_ComponentIsSet()
    {
      using World world = new(0);
      Entity entity = world.CreateEntity();
      UnittestComponent component = new(1);

      world.Set(entity.Id, component);
      world.Get<UnittestComponent>(entity.Id).Should().Be(component);
    }

    [Fact]
    public void Set_EntityHavingComponent_ComponentIsOverwritten()
    {
      using World world = new(0);
      Entity entity = world.CreateEntity();
      UnittestComponent component = new(1);
      UnittestComponent component2 = new(2);

      world.Set(entity.Id, component);
      world.Set(entity.Id, component2);
      world.Get<UnittestComponent>(entity.Id).Should().Be(component2);
    }
    [Fact]
    public void Remove_EntityNotHavingComponent_NotThrowAnException()
    {
      using World world = new(0);
      Entity entity = world.CreateEntity();
      Action action = () => world.Remove<UnittestComponent>(entity.Id);

      action.Should().Throw<MappingNotFoundException>();
    }
    [Fact]
    public void Remove_EntityHavingComponent_ComponentRemoved()
    {
      using World world = new(0);
      Entity entity = world.CreateEntity();
      UnittestComponent component = new(1);

      world.Set(entity.Id, component);
      world.Remove<UnittestComponent>(entity.Id);

      world.Has<UnittestComponent>(entity.Id).Should().BeFalse();
    }
    [Fact]
    public void IterateEntities_WorldHasEntities_AllEntitiesAreReturned()
    {
      using World world = new(0);
      Mock<Action<Entity>> entityActionMock = new();
      List<Entity> expectedResult = new()
      {
        world.CreateEntity(),
        world.CreateEntity(),
        world.CreateEntity()
      };
      List<Entity> result = new();

      entityActionMock.Setup(_ => _.Invoke(It.IsAny<Entity>()))
                      .Callback(new Action<Entity>((Entity _) => result.Add(_)));

      world.IterateEntities(entityActionMock.Object, FilterExpressions.All());

      entityActionMock.Verify(_ => _.Invoke(It.IsAny<Entity>()), Times.Exactly(3));
      expectedResult.Should().BeEquivalentTo(result);
    }
  }
}

using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Valkyr.ECS.Tests
{
  public class ContextTest
  {
    public ContextTest()
    {
      UnittestSystem.UpdatedEntities.Clear();
    }

    [Fact]
    public void CreateWorld_None_WorldSuccessfullyCreated()
    {
      using Context<int> context = new();
      IWorld world = context.CreateWorld();

      world.Should().NotBeNull();
      context.HasWorld(world.Id).Should().BeTrue();
    }
    [Fact]
    public void DestroyWorld_ValidWorld_WorldSuccessfullyDestroyed()
    {
      using Context<int> context = new();
      IWorld world = context.CreateWorld();

      context.DestroyWorld(world);
      context.HasWorld(world.Id).Should().BeFalse();
    }
    [Fact]
    public void DestroyWorld_ValidWorldId_WorldSuccessfullyDestroyed()
    {
      using Context<int> context = new();
      IWorld world = context.CreateWorld();

      context.DestroyWorld(world.Id);
      context.HasWorld(world.Id).Should().BeFalse();
    }
    [Fact]
    public void RegisterSystem_ValidSystem_SystemIsSuccessfullyRegistered()
    {
      using Context<int> context = new();

      context.RegisterSystem<UnittestSystem>();
      context.IsRegistered<UnittestSystem>().Should().BeTrue();
    }
    [Fact]
    public void RegisterSystem_MultipleValidSystems_SystemsAreSuccessfullyRegistered()
    {
      using Context<int> context = new();

      context.RegisterSystem<UnittestSystem>();
      context.RegisterSystem<UnittestSystem2>();
      context.IsRegistered<UnittestSystem>().Should().BeTrue();
      context.IsRegistered<UnittestSystem2>().Should().BeTrue();
    }
    [Fact]
    public void UnregisterSystem_ValidSystem_SystemIsSuccessfullyUnregistered()
    {
      using Context<int> context = new();

      context.RegisterSystem<UnittestSystem>();
      context.UnregisterSystem<UnittestSystem>();
      context.IsRegistered<UnittestSystem>().Should().BeFalse();
    }
    [Fact]
    public void UnregisterSystem_MultipleValidSystems_SystemAreSuccessfullyUnregistered()
    {
      using Context<int> context = new();

      context.RegisterSystem<UnittestSystem>();
      context.RegisterSystem<UnittestSystem2>();
      context.UnregisterSystem<UnittestSystem>();
      context.UnregisterSystem<UnittestSystem2>();
      context.IsRegistered<UnittestSystem>().Should().BeFalse();
      context.IsRegistered<UnittestSystem2>().Should().BeFalse();
    }
    [Fact]
    public void UnregisterSystem_NotRegisteredSystem_InvalidOperationException()
    {
      using Context<int> context = new();
      Action action = () => context.UnregisterSystem<UnittestSystem>();

      action.Should().Throw<InvalidOperationException>();
    }
    [Fact]
    public void EnableSystem_ValidSystem_SystemIsSuccessfullyEnabled()
    {
      using Context<int> context = new();

      context.RegisterSystem<UnittestSystem>();
      context.EnableSystem<UnittestSystem>();
      context.IsEnabled<UnittestSystem>().Should().BeTrue();
    }
    [Fact]
    public void EnableSystem_NotRegisteredSystem_InvalidOperationException()
    {
      using Context<int> context = new();
      Action action = () => context.EnableSystem<UnittestSystem>();

      action.Should().Throw<InvalidOperationException>();
    }
    [Fact]
    public void DisableSystem_ValidSystem_SystemIsSuccessfullyDisabled()
    {
      using Context<int> context = new();

      context.RegisterSystem<UnittestSystem>();
      context.EnableSystem<UnittestSystem>();
      context.DisableSystem<UnittestSystem>();
      context.IsEnabled<UnittestSystem>().Should().BeFalse();
    }
    [Fact]
    public void DisableSystem_NotRegisteredSystem_InvalidOperationException()
    {
      using Context<int> context = new();
      Action action = () => context.DisableSystem<UnittestSystem>();

      action.Should().Throw<InvalidOperationException>();
    }
    [Fact]
    public void Update_None_AllWorldsUpdated()
    {
      using Context<int> context = new();
      IWorld world = context.CreateWorld();
      IWorld world1 = context.CreateWorld();
      List<Entity> expectedEntities = new()
      {
        world.CreateEntity(),
        world.CreateEntity(),
        world1.CreateEntity(),
        world1.CreateEntity(),
      };

      context.RegisterSystem<UnittestSystem>();

      context.Update(1);

      UnittestSystem.UpdatedEntities.Should().ContainInOrder(expectedEntities);
    }

    [Fact]
    public void Update_None_AllActiveWorldsUpdated()
    {
      using Context<int> context = new();
      IWorld world = context.CreateWorld();
      IWorld world2 = context.CreateWorld();
      Entity entity = world2.CreateEntity();
      List<Entity> expectedEntities = new()
      {
        world.CreateEntity(),
        world.CreateEntity()
      };


      world2.Active = false;

      context.RegisterSystem<UnittestSystem>();

      context.Update(1);

      UnittestSystem.UpdatedEntities.Should().ContainInOrder(expectedEntities);
      UnittestSystem.UpdatedEntities.Should().NotContain(entity);
    }
  }
}

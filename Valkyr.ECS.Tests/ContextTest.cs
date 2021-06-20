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
      UnittestSystem.Reset();
    }

    [Fact]
    public void CreateWorld_None_WorldSuccessfullyCreated()
    {
      using Context context = new();
      IWorld world = context.CreateWorld();

      world.Should().NotBeNull();
      context.HasWorld(world.Id).Should().BeTrue();
    }
    [Fact]
    public void DestroyWorld_ValidWorld_WorldSuccessfullyDestroyed()
    {
      using Context context = new();
      IWorld world = context.CreateWorld();

      context.DestroyWorld(world);
      context.HasWorld(world.Id).Should().BeFalse();
    }
    [Fact]
    public void DestroyWorld_ValidWorldId_WorldSuccessfullyDestroyed()
    {
      using Context context = new();
      IWorld world = context.CreateWorld();

      context.DestroyWorld(world.Id);
      context.HasWorld(world.Id).Should().BeFalse();
    }
    [Fact]
    public void RegisterSystem_ValidSystem_SystemIsSuccessfullyRegistered()
    {
      using Context context = new();

      context.RegisterSystem<UnittestSystem>();
      context.IsRegistered<UnittestSystem>().Should().BeTrue();
    }
    [Fact]
    public void UnregisterSystem_ValidSystem_SystemIsSuccessfullyUnregistered()
    {
      using Context context = new();

      context.RegisterSystem<UnittestSystem>();
      context.UnregisterSystem<UnittestSystem>();
      context.IsRegistered<UnittestSystem>().Should().BeFalse();
    }
    [Fact]
    public void UnregisterSystem_NotRegisteredSystem_InvalidOperationException()
    {
      using Context context = new();
      Action action = () => context.UnregisterSystem<UnittestSystem>();

      action.Should().Throw<InvalidOperationException>();
    }
    [Fact]
    public void EnableSystem_ValidSystem_SystemIsSuccessfullyEnabled()
    {
      using Context context = new();

      context.RegisterSystem<UnittestSystem>();
      context.EnableSystem<UnittestSystem>();
      context.IsEnabled<UnittestSystem>().Should().BeTrue();
    }
    [Fact]
    public void EnableSystem_NotRegisteredSystem_InvalidOperationException()
    {
      using Context context = new();
      Action action = () => context.EnableSystem<UnittestSystem>();

      action.Should().Throw<InvalidOperationException>();
    }
    [Fact]
    public void DisableSystem_ValidSystem_SystemIsSuccessfullyDisabled()
    {
      using Context context = new();

      context.RegisterSystem<UnittestSystem>();
      context.EnableSystem<UnittestSystem>();
      context.DisableSystem<UnittestSystem>();
      context.IsEnabled<UnittestSystem>().Should().BeFalse();
    }
    [Fact]
    public void DisableSystem_NotRegisteredSystem_InvalidOperationException()
    {
      using Context context = new();
      Action action = () => context.DisableSystem<UnittestSystem>();

      action.Should().Throw<InvalidOperationException>();
    }
    [Fact]
    public void Update_None_AllWorldsUpdated()
    {
      using Context context = new();
      List<IWorld> expectedWorlds = new()
      {
        context.CreateWorld(),
        context.CreateWorld()
      };

      context.RegisterSystem<UnittestSystem>();

      context.Update();

      UnittestSystem.UpdatedWorlds.Should().ContainInOrder(expectedWorlds);
    }

    [Fact]
    public void Update_None_AllActiveWorldsUpdated()
    {
      using Context context = new();
      List<IWorld> expectedWorlds = new()
      {
        context.CreateWorld(),
        context.CreateWorld()
      };
      IWorld world = context.CreateWorld();

      world.Active = false;

      context.RegisterSystem<UnittestSystem>();

      context.Update();

      UnittestSystem.UpdatedWorlds.Should().ContainInOrder(expectedWorlds);
      UnittestSystem.UpdatedWorlds.Should().NotContain(world);
    }
  }
}

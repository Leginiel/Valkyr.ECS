using FluentAssertions;
using System;
using Xunit;

namespace Valkyr.ECS.Tests
{
  public class ComponentPoolTest
  {

    [Fact]
    public void HasCapacity_PoolHasCapacity_True()
    {
      IComponentPool<UnittestComponent> pool = new ComponentPool<UnittestComponent>();

      pool.HasCapacity().Should().BeTrue();
    }
    [Fact]
    public void HasCapacity_PoolHasNoCapacity_False()
    {
      IComponentPool<UnittestComponent> pool = new ComponentPool<UnittestComponent>(0);

      pool.HasCapacity().Should().BeFalse();
    }
    [Fact]
    public void Store_PoolHasCapoacity_ComponentStored()
    {
      IComponentPool<UnittestComponent> pool = new ComponentPool<UnittestComponent>();
      UnittestComponent component = new(1);

      pool.Store(1, component).Should().Be(component);
      pool.Count.Should().Be(1);
    }
    [Fact]
    public void Store_PoolHasNoCapoacity_ThrowsMaximumCapacityReached()
    {
      IComponentPool<UnittestComponent> pool = new ComponentPool<UnittestComponent>(0);
      UnittestComponent component = new(1);
      Action action = () => pool.Store(1, component);

      action.Should().ThrowExactly<MaximumCapacityReachedException>();
      pool.Count.Should().Be(0);
    }
    [Fact]
    public void Has_PoolHasComponent_True()
    {
      IComponentPool<UnittestComponent> pool = new ComponentPool<UnittestComponent>();
      UnittestComponent component = new(1);
      pool.Store(1, component);


      pool.Has(1).Should().BeTrue();
    }
    [Fact]
    public void Has_PoolHasNotComponent_False()
    {
      IComponentPool<UnittestComponent> pool = new ComponentPool<UnittestComponent>();

      pool.Has(1).Should().BeFalse();
    }
    [Fact]
    public void Receive_NoItemStored_MappingNotFoundException()
    {
      IComponentPool<UnittestComponent> pool = new ComponentPool<UnittestComponent>(0);
      Action action = () => pool.Receive(1);

      action.Should().ThrowExactly<MappingNotFoundException>();
    }
    [Fact]
    public void Receive_ItemStored_Item()
    {
      IComponentPool<UnittestComponent> pool = new ComponentPool<UnittestComponent>();
      UnittestComponent component = new(1);

      pool.Store(1, in component);
      pool.Receive(1).Should().Be(component);
    }
    [Fact]
    public void Remove_ItemStored_True()
    {
      IComponentPool<UnittestComponent> pool = new ComponentPool<UnittestComponent>();
      UnittestComponent component = new(1);

      pool.Store(1, in component);
      pool.Remove(1);
      pool.Count.Should().Be(0);
    }
    [Fact]
    public void Remove_ItemNotStored_False()
    {
      IComponentPool<UnittestComponent> pool = new ComponentPool<UnittestComponent>();
      Action action = () => pool.Remove(1);

      action.Should().ThrowExactly<MappingNotFoundException>();
    }
  }
}

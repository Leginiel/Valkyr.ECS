using FluentAssertions;
using System;
using Xunit;

namespace Valkyr.ECS.Tests
{
  public class PoolTest
  {

    [Fact]
    public void HasCapacity_PoolHasCapacity_True()
    {
      IPool<UnittestComponent> pool = new Pool<UnittestComponent>();

      pool.HasCapacity().Should().BeTrue();
    }
    [Fact]
    public void HasCapacity_PoolHasNoCapacity_False()
    {
      IPool<UnittestComponent> pool = new Pool<UnittestComponent>(0);

      pool.HasCapacity().Should().BeFalse();
    }
    [Fact]
    public void Store_PoolHasCapoacity_ComponentStored()
    {
      IPool<UnittestComponent> pool = new Pool<UnittestComponent>();
      UnittestComponent component = new(1);

      pool.Store(1, component).Should().Be(component);
      pool.Count.Should().Be(1);
    }
    [Fact]
    public void Store_PoolHasNoCapoacity_ThrowsMaximumCapacityReached()
    {
      IPool<UnittestComponent> pool = new Pool<UnittestComponent>(0);
      UnittestComponent component = new(1);
      Action action = () => pool.Store(1, component);

      action.Should().ThrowExactly<MaximumCapacityReachedException>();
      pool.Count.Should().Be(0);
    }
    [Fact]
    public void Receive_NoItemStored_MappingNotFoundException()
    {
      IPool<UnittestComponent> pool = new Pool<UnittestComponent>(0);
      Action action = () => pool.Receive(1);

      action.Should().ThrowExactly<MappingNotFoundException>();
    }
    [Fact]
    public void Receive_ItemStored_Item()
    {
      IPool<UnittestComponent> pool = new Pool<UnittestComponent>();
      UnittestComponent component = new(1);

      pool.Store(1, in component);
      pool.Receive(1).Should().Be(component);
    }
    [Fact]
    public void Remove_ItemStored_True()
    {
      IPool<UnittestComponent> pool = new Pool<UnittestComponent>();
      UnittestComponent component = new(1);

      pool.Store(1, in component);
      pool.Remove(1);
      pool.Count.Should().Be(0);
    }
    [Fact]
    public void Remove_ItemNotStored_False()
    {
      IPool<UnittestComponent> pool = new Pool<UnittestComponent>();
      Action action = () => pool.Remove(1);

      action.Should().ThrowExactly<MappingNotFoundException>();
    }
  }
}

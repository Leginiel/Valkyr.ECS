using FluentAssertions;
using Xunit;

namespace Valkyr.ECS.Tests
{
  public class PoolTest
  {

    [Fact]
    public void HasCapacity_PoolHasCapacity_True()
    {
      IPool pool = Pool.Create<UnittestComponent>();

      pool.HasCapacity().Should().BeTrue();
    }
    [Fact]
    public void HasCapacity_PoolHasNoCapacity_False()
    {
      IPool pool = Pool.Create<UnittestComponent>(0);

      pool.HasCapacity().Should().BeFalse();
    }
    [Fact]
    public void TryStore_PoolHasCapoacity_True()
    {
      IPool pool = Pool.Create<UnittestComponent>();
      UnittestComponent component = new(1);

      pool.TryStore(1, component).Should().BeTrue();
    }
    [Fact]
    public void TryStore_PoolHasWrongType_False()
    {
      IPool pool = Pool.Create<Entity>();
      UnittestComponent component = new(1);

      pool.TryStore(1, component).Should().BeFalse();
    }
    [Fact]
    public void TryStore_PoolHasNoCapoacity_False()
    {
      IPool pool = Pool.Create<UnittestComponent>(0);
      UnittestComponent component = new(1);

      pool.TryStore(1, component).Should().BeFalse();
    }
    [Fact]
    public void Receive_NoItemStored_Default()
    {
      IPool pool = Pool.Create<UnittestComponent>(0);

      pool.TryReceive(1, out UnittestComponent result).Should().Be(false);
      result.Should().Be(default(UnittestComponent));
    }
    [Fact]
    public void Receive_ItemStored_Item()
    {
      IPool pool = Pool.Create<UnittestComponent>();
      UnittestComponent component = new(1);

      pool.TryStore(1, in component);
      pool.TryReceive(1, out UnittestComponent result).Should().Be(true);
      result.Should().Be(component);

    }
  }
}

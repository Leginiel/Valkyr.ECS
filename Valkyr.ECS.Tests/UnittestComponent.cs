namespace Valkyr.ECS.Tests
{
  public readonly struct UnittestComponent : IComponent
  {
    public readonly int Value;

    public UnittestComponent(int value)
    {
      Value = value;
    }
  }
}

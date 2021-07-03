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
  public readonly struct UnittestComponent2 : IComponent
  {
    public readonly int Value;

    public UnittestComponent2(int value)
    {
      Value = value;
    }
  }
}

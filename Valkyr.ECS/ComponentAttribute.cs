using System;

namespace Valkyr.ECS
{
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
  public sealed class ComponentAttribute : Attribute
  {
    public string Name { get; }
    public ComponentAttribute(string name)
    {
      Name = name;
    }
  }
}

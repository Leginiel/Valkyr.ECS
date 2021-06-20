using System;
using System.Collections.Generic;

namespace Valkyr.ECS
{
  public static class FilterExpressions
  {
    private static IFilterExpression all;
    private static Dictionary<Type, IFilterExpression> componentFilter = new();

    public static IFilterExpression All() => all ??= new AllFilterExpression();
    public static IFilterExpression Component<T>()
      where T : IComponent
    {

      Type type = typeof(T);

      if (!componentFilter.ContainsKey(type))
      {
        componentFilter.Add(type, new ComponentFilterExpression<T>());
      }
      return componentFilter[type];
    }
  }
}

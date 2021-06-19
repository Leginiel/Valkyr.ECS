namespace Valkyr.ECS
{
  public static class FilterExpressions
  {
    private static IFilterExpression all;

    public static IFilterExpression All => all ??= new AllFilterExpression();
  }
}

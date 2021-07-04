namespace Valkyr.ECS
{
  public class AllFilterExpression : IFilterExpression
  {
    public bool Matches<T>(T element)
    {
      return true;
    }
  }
}

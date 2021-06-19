namespace Valkyr.ECS
{
  public class AllFilterExpression : IFilterExpression
  {
    public bool Matches<T>(ref T element)
    {
      return true;
    }
  }
}

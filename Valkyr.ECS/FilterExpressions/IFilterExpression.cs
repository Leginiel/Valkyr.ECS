namespace Valkyr.ECS
{
  public interface IFilterExpression
  {
    bool Matches<T>(ref T element);
  }
}
namespace Valkyr.ECS
{
  public interface IFilterExpression
  {
    bool Matches<T>(T element);
  }
}
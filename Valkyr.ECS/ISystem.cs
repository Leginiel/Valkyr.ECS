namespace Valkyr.ECS
{
  public interface ISystem
  {
    public bool Enabled { get; set; }
    public void Update();
  }
}

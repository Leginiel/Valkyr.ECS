namespace Valkyr.ECS
{
  public abstract class System<T> : ISystem
    where T : struct, IComponent
  {
    private readonly IWorld world;

    public bool Enabled { get; set; }

    public System(IWorld world)
    {
      this.world = world;
    }

    protected abstract void Update(ref Entity entity);
    public void Update()
    {
      world.IterateEntities(Update, FilterExpressions.All());
    }
  }
}

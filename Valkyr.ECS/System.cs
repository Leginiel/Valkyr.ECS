namespace Valkyr.ECS
{
  public abstract class System<T> : ISystem
    where T : struct, IComponent
  {
    public bool Enabled { get; set; }

    protected abstract void Update(ref Entity entity);
    public virtual void Update(IWorld world)
    {
      world.IterateEntities(Update, FilterExpressions.All());
    }
  }
}

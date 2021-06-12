using System;

namespace Valkyr.ECS
{
  public class MappingNotFoundException : Exception
  {
    public MappingNotFoundException(int id)
      : base($"No Mapping exists for Id {id}") { }
  }
}

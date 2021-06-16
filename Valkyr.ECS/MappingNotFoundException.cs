using System;
using System.Runtime.Serialization;

namespace Valkyr.ECS
{
  [Serializable]
  public class MappingNotFoundException : Exception
  {
    public MappingNotFoundException(int id)
      : base($"No Mapping exists for Id {id}") { }
    protected MappingNotFoundException(SerializationInfo info, StreamingContext context)
      : base(info, context) { }
  }
}

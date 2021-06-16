using System;
using System.Runtime.Serialization;

namespace Valkyr.ECS
{
  [Serializable]
  public class MaximumCapacityReachedException : Exception
  {
    public MaximumCapacityReachedException(int maxCapacity)
    : base($"Maximum capacity of {maxCapacity} for storage reached") { }
    protected MaximumCapacityReachedException(SerializationInfo info, StreamingContext context)
      : base(info, context) { }
  }
}

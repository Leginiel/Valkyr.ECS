using System;

namespace Valkyr.ECS
{
  [Serializable]
  public class MaximumCapacityReachedException : Exception
  {
    public MaximumCapacityReachedException(int maxCapacity)
    : base($"Maximum capacity of {maxCapacity} for storage reached") { }
  }
}

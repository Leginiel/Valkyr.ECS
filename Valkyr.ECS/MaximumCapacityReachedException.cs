using System;

namespace Valkyr.ECS
{
  public class MaximumCapacityReachedException : Exception
  {
    public MaximumCapacityReachedException(int maxCapacity)
    : base($"Maximum capacity of {maxCapacity} for storage reached") { }
  }
}

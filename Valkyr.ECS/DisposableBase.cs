using System;

namespace Valkyr.ECS
{
  public class DisposableBase : IDisposable
  {
    private bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
      if (!disposedValue)
      {
        disposedValue = true;
      }
    }

    public void Dispose()
    {
      Dispose(disposing: true);
      GC.SuppressFinalize(this);
    }
  }
}

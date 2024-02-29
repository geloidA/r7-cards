using System.Timers;
using Cardmngr.Exceptions;
using Timer = System.Timers.Timer;

namespace Cardmngr.Utils;

public class RefreshService : IDisposable
{
    private bool _started = false;
    private bool _disposed = false;
    private readonly Timer timer = new();
    private readonly HashSet<Guid> locks = [];

    public void Start(TimeSpan refreshInterval)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        if (_started) throw new RefreshServiceMultipleStartException();

        timer.Interval = refreshInterval.TotalMilliseconds;
        timer.Elapsed += OnRefreshElapsed;

        timer.Enabled = _started = true;
    }

    public void RemoveLock(Guid lockGuid)
    {
        if (locks.Remove(lockGuid))
        {
            if (locks.Count == 0)
                timer.Enabled = true;
        }
    }

    public void Lock(Guid lockGuid)
    {
        locks.Add(lockGuid);
        timer.Enabled = false;
    }

    public bool Enabled => timer.Enabled;

    public event Action? Refreshed;
    private void OnRefreshElapsed(object? sender, ElapsedEventArgs e) => Refreshed?.Invoke();

    public void Dispose()
    {
        Refreshed = null;
        timer.Dispose();
        _disposed = true;
    }
}

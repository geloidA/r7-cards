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

        _started = true;
        timer.Enabled = locks.Count == 0;
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
        if (lockGuid == Guid.Empty) throw new ArgumentException("lockGuid cannot be empty");
        locks.Add(lockGuid);
        timer.Enabled = false;
    }

    public bool Enabled => timer.Enabled;
    public bool Started => _started;
    public bool Disposed => _disposed;

    public event Action? Refreshed;
    private void OnRefreshElapsed(object? sender, ElapsedEventArgs e) => Refreshed?.Invoke();

    public void Dispose()
    {
        Refreshed = null;
        timer.Dispose();
        _disposed = true;
    }
}

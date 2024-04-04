using Cardmngr.Exceptions;

namespace Cardmngr.Utils;

public class RefreshService : IDisposable
{
    private bool _started = false;
    private bool _disposed = false;
    private Timer? timer;
    private readonly HashSet<Guid> locks = [];
    private TimeSpan refreshInterval;

    public void Start(TimeSpan refreshInterval)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        if (_started) throw new RefreshServiceMultipleStartException();

        this.refreshInterval = refreshInterval;

        timer = locks.Count == 0
            ? new Timer(OnRefreshElapsed, null, refreshInterval, refreshInterval)
            : new Timer(OnRefreshElapsed, null, Timeout.Infinite, Timeout.Infinite);
    
        _started = true;
    }

    public void RemoveLock(Guid lockGuid)
    {
        if (locks.Remove(lockGuid))
        {
            if (locks.Count == 0)
                timer?.Change(refreshInterval, refreshInterval);
        }
    }

    public void Lock(Guid lockGuid)
    {
        if (lockGuid == Guid.Empty) throw new ArgumentException("lockGuid cannot be empty");
        locks.Add(lockGuid);
        if (locks.Count == 1)
            timer?.Change(Timeout.Infinite, Timeout.Infinite);
    }

    public bool Enabled => locks.Count == 0 && _started;
    public bool Started => _started;
    public bool Disposed => _disposed;

    public event Action? Refreshed;
    private void OnRefreshElapsed(object? sender) => Refreshed?.Invoke();

    public void Dispose()
    {
        Refreshed = null;
        timer?.Dispose();
        _disposed = true;
    }
}

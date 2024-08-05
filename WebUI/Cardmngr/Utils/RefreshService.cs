using Cardmngr.Exceptions;

namespace Cardmngr.Utils;

public sealed class RefreshService : IDisposable
{
    private Timer? timer;
    private readonly HashSet<Guid> locks = [];
    private TimeSpan _refreshInterval;

    public void Start(TimeSpan refreshInterval)
    {
        ObjectDisposedException.ThrowIf(Disposed, this);
        if (Started) throw new RefreshServiceMultipleStartException();

        _refreshInterval = refreshInterval;

        timer = locks.Count == 0
            ? new Timer(OnRefreshElapsed, null, refreshInterval, refreshInterval)
            : new Timer(OnRefreshElapsed, null, Timeout.Infinite, Timeout.Infinite);
    
        Started = true;
    }

    public void RemoveLock(Guid lockGuid)
    {
        if (!locks.Remove(lockGuid)) return;
        if (locks.Count == 0)
            timer?.Change(_refreshInterval, _refreshInterval);
    }

    public void Lock(Guid lockGuid)
    {
        if (lockGuid == Guid.Empty) throw new ArgumentException("lockGuid cannot be empty");
        locks.Add(lockGuid);
        if (locks.Count == 1)
            timer?.Change(Timeout.Infinite, Timeout.Infinite);
    }

    public bool Enabled => locks.Count == 0 && Started;
    public bool Started { get; private set; }

    public bool Disposed { get; private set; }

    public event Action? Refreshed;
    private void OnRefreshElapsed(object? sender) => Refreshed?.Invoke();

    public void Dispose()
    {
        Refreshed = null;
        timer?.Dispose();
        Disposed = true;
    }
}

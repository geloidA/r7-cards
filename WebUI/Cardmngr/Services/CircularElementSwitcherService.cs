using System.Timers;

namespace Cardmngr.Services;

public class CircularElementSwitcherService<T> : ICircularElementSwitcherService<T>
{
    private readonly System.Timers.Timer _switchTimer = new();
    private DateTime _elementFirstShowTime;
    private int _currentElementIndex;
    private T[]? _elements;

    private T[] Elements => _elements ?? throw new InvalidOperationException("The elements are not set.");

    public T Element => Elements[_currentElementIndex];

    public double MillisecondsElapsed
    {
        get
        {
            if (Stopped)
            {
                return 0;
            }

            return (DateTime.Now - _elementFirstShowTime).TotalMilliseconds;
        }
    }

    private int _switchInterval = 1000;

    public event Action? OnElementChanged;
    private void ElementChanged() => OnElementChanged?.Invoke();

    public CircularElementSwitcherService()
    {
        _switchTimer.Elapsed += OnSwitchTimerElapsed;
        _switchTimer.Interval = _switchInterval;
    }

    public int SwitchInterval 
    { 
        get => _switchInterval; 
        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(SwitchInterval), "The switch interval must be positive.");
            }

            _switchInterval = value;
            _switchTimer.Interval = _switchInterval;
        }
    }

    public bool Stopped => !_switchTimer.Enabled;

    private void OnSwitchTimerElapsed(object? sender, ElapsedEventArgs e) => Next();

    public void Next() => ResetTimerElapsing(() => _currentElementIndex = (_currentElementIndex + 1) % Elements.Length);

    public void Previous() => ResetTimerElapsing(() => _currentElementIndex = (_currentElementIndex - 1 + Elements.Length) % Elements.Length);

    private void ResetTimerElapsing(Action callback)
    {
        var previousEnabledState = _switchTimer.Enabled;
        _switchTimer.Enabled = false;
        callback();
        _switchTimer.Enabled = previousEnabledState;
        _elementFirstShowTime = DateTime.Now;
        ElementChanged();
    }

    public void Stop()
    {
        _switchTimer.Enabled = false;
    }

    public void Start()
    {
        _elementFirstShowTime = DateTime.Now;
        _switchTimer.Enabled = true;
    }

    public void Dispose() => _switchTimer.Dispose();

    public void SetElements(IEnumerable<T> elements)
    {
        _elements = elements.ToArray();
        _currentElementIndex = 0;
    }
}

public interface ICircularElementSwitcherService<T> : IDisposable
{
    /// <summary>
    /// Gets the current element.
    /// </summary>
    T Element { get; }

    event Action? OnElementChanged;

    /// <summary>
    /// Gets the time elapsed in milliseconds.
    /// </summary>
    double MillisecondsElapsed { get; }

    /// <summary>
    /// Switches the current element to the next. In milliseconds. Default is 1000.
    /// </summary>
    int SwitchInterval { get; set; }

    bool Stopped { get; }

    void Next();
    void Previous();
    void Stop();
    void Start();

    public void SetElements(IEnumerable<T> elements);
}
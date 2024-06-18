using System.Timers;
using Cardmngr.Components.ProjectAggregate.States;
using Cardmngr.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Cardmngr.Pages;

public partial class ProjectSummaryInfoPage : ComponentBase
{
    private readonly Guid _lockGuid = Guid.NewGuid();
    private SummaryInfoProjectState _currentState = null!;
    private readonly System.Timers.Timer _timer = new();
    private readonly System.Timers.Timer _progressTimer = new() { Interval = 1000 };
    private int _progress = 0;
    private int _progressMaxValue;
    private int _currentProjectId = 0;

    [SupplyParameterFromQuery]
    public int MeasurementUnit { get; set; }

    [SupplyParameterFromQuery]
    public int ChangeInterval { get; set; }

    [SupplyParameterFromQuery(Name = "projects")]
    public int[]? Projects { get; set; }

    protected override void OnInitialized()
    {
        _progressTimer.Elapsed += OnProgressTimerElapsed;
        _progressTimer.Start();
        
        _timer.Elapsed += OnTimerElapsed;
        _timer.Start();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender && _currentState is { })
        {
            _currentState.OnAfterIdChanged += state => 
            {
                (_timer.Interval, _progressMaxValue) = state.Tasks.Count == 0
                    ? (3000, 3)
                    : GetIntervalAndMaxValue();
                StateHasChanged();
            };
        }
    }

    private (int, int) GetIntervalAndMaxValue() => MeasurementUnit == (int)TimeMeasurementUnit.Minutes
        ? (ChangeInterval * 60 * 1000, ChangeInterval * 60)
        : (ChangeInterval * 1000, ChangeInterval);

    private bool _smoothShowing = true;

    private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        NextProject();
    }

    private void NextProject()
    {
        ResetTimers(() => _currentProjectId = (_currentProjectId + 1) % Projects!.Length);
    }

    private void PreviousProject()
    {
        ResetTimers(() => _currentProjectId = (_currentProjectId - 1 + Projects!.Length) % Projects.Length);
    }

    private void ResetTimers(Action action)
    {
        var prevEnabled = _timer.Enabled;
        _timer.Enabled = false;

        _progressTimer.Enabled = false;
        _progressTimer.Enabled = prevEnabled;
        _progress = 0;

        _smoothShowing = false; // hide
        StateHasChanged();

        action();

        _smoothShowing = true; // show

        _timer.Enabled = prevEnabled;
    }
    
    private void PauseTimer()
    {
        _timer.Enabled = false;
        _progressTimer.Enabled = false;

        if (!_currentState.RefreshService.Started)
        {
            _currentState.RefreshService.Start(TimeSpan.FromSeconds(7));
        }
        else
        {
            _currentState.RefreshService.RemoveLock(_lockGuid);
        }
    }

    private void ResumeTimer()
    {
        _timer.Enabled = true;
        _progressTimer.Enabled = true;

        _currentState.RefreshService.Lock(_lockGuid);
    }

    private void OnProgressTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        _progress++;
        StateHasChanged();
    }

    public void Dispose()
    {
        _timer.Dispose();
        _progressTimer.Dispose();
    }
}

using System.Timers;
using Cardmngr.Utils;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Pages;

public partial class ProjectDashboardsPage : ComponentBase, IDisposable
{
    private readonly System.Timers.Timer _timer = new();
    private readonly System.Timers.Timer _progressTimer = new() { Interval = 1000 };
    private int _progress = 0;
    private int _currentProjectId = 0;

    [SupplyParameterFromQuery]
    public int MeasurementUnit { get; set; }

    [SupplyParameterFromQuery]
    public int ChangeInterval { get; set; }

    [SupplyParameterFromQuery(Name = "projects")]
    public int[]? Projects { get; set; }

    private int ProgressMaxValue => MeasurementUnit == (int)TimeMeasurementUnit.Minutes 
        ? ChangeInterval * 60
        : ChangeInterval;

    protected override void OnInitialized()
    {
        var unit = (TimeMeasurementUnit)MeasurementUnit;
        _timer.Interval = unit == TimeMeasurementUnit.Minutes
            ? ChangeInterval * 60 * 1000
            : ChangeInterval * 1000;
        
        _progressTimer.Elapsed += OnProgressTimerElapsed;
        _progressTimer.Start();
        
        _timer.Elapsed += OnTimerElapsed;
        _timer.Start();
    }

    private bool _smoothShowing = true;

    private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        _progressTimer.Enabled = false;
        _progressTimer.Enabled = true;
        _progress = 0;

        _smoothShowing = false; // hide
        StateHasChanged();

        _currentProjectId = (_currentProjectId  + 1) % Projects!.Length;

        _smoothShowing = true; // show
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

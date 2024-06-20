using System.Timers;
using Cardmngr.Components.ProjectAggregate.States;
using Cardmngr.Services;
using Cardmngr.Utils;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Pages;

public partial class ProjectSummaryInfoPage : ComponentBase, IDisposable
{
    private readonly Guid _lockGuid = Guid.NewGuid();
    private readonly System.Timers.Timer _timer = new() { Interval = 100};
    private SummaryInfoProjectState _currentState = null!;

    [SupplyParameterFromQuery]
    public int MeasurementUnit { get; set; }

    [SupplyParameterFromQuery]
    public int ChangeInterval { get; set; }

    [SupplyParameterFromQuery(Name = "projects")]
    public int[]? Projects { get; set; }

    [Inject] ICircularElementSwitcherService<int> ElementSwitcherService { get; set; } = null!;

    protected override void OnInitialized()
    {
        _timer.Elapsed += (_, _) => StateHasChanged();
        _timer.Start();

        if (Projects is { })
        {
            ElementSwitcherService.SetElements(Projects);
            ElementSwitcherService.OnElementChanged += StateHasChanged;
            ElementSwitcherService.Start();
        }
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender && _currentState is { })
        {
            _currentState.OnAfterIdChanged += state => 
            {
                ElementSwitcherService.SwitchInterval = state.Tasks.Count != 0 ? SwitchInterval : 3000;
                StateHasChanged();
            };
        }
    }

    private int SwitchInterval => MeasurementUnit == (int)TimeMeasurementUnit.Minutes
        ? ChangeInterval * 60 * 1000
        : ChangeInterval * 1000;

    private bool _smoothShowing = true;

    private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        NextProject();
    }

    private async void NextProject()
    {
        _smoothShowing = false; // hide
        StateHasChanged();

        await Task.Delay(300);

        ElementSwitcherService.Next();

        _smoothShowing = true; // show
    }

    private async void PreviousProject()
    {
        _smoothShowing = false; // hide
        StateHasChanged();

        await Task.Delay(300);

        ElementSwitcherService.Previous();

        _smoothShowing = true; // show
    }
    
    private void PauseTimer()
    {
        ElementSwitcherService.Stop();

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
        ElementSwitcherService.Start();
        _currentState.RefreshService.Lock(_lockGuid);
    }

    public void Dispose()
    {
        _timer.Dispose();
    }
}
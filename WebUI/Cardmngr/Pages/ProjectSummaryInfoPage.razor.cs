﻿using Cardmngr.Components.ProjectAggregate.States;
using Cardmngr.Services;
using Cardmngr.Utils;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Pages;

public sealed partial class ProjectSummaryInfoPage : ComponentBase, IDisposable
{
    private readonly Guid _lockGuid = Guid.NewGuid();
    private readonly System.Timers.Timer _timer = new() { Interval = 100 };
    private SummaryInfoProjectState _currentState = null!;

    [SupplyParameterFromQuery]
    public int MeasurementUnit { get; set; }

    [SupplyParameterFromQuery]
    public int ChangeInterval { get; set; }

    [SupplyParameterFromQuery(Name = "projects")]
    public int[]? Projects { get; set; }

    [Inject] private ICircularElementSwitcherService<int> ElementSwitcherService { get; set; } = null!;

    protected override void OnInitialized()
    {
        _timer.Elapsed += (_, _) => StateHasChanged();
        _timer.Start();

        if (Projects is null) return;
        ElementSwitcherService.SetElements(Projects);
        ElementSwitcherService.OnElementChanged += () =>
        {
            StateHasChanged();
            ElementSwitcherService.BlockSwitch = true;
        };
        ElementSwitcherService.Start();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
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

    private void CanSwitchHandler()
    {
        ElementSwitcherService.BlockSwitch = false;
    }

    private async Task NextProject()
    {
        _smoothShowing = false; // hide

        await Task.Delay(300);
        ElementSwitcherService.Next();
        StateHasChanged();

        _smoothShowing = true; // show
    }

    private async Task PreviousProject()
    {
        _smoothShowing = false; // hide

        await Task.Delay(300);
        ElementSwitcherService.Previous();
        StateHasChanged();

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
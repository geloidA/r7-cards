using Cardmngr.Components.ProjectAggregate.States;
using Cardmngr.Domain.Entities;
using Cardmngr.Extensions;
using Cardmngr.Shared.Utils.Filter.TaskFilters;
using KolBlazor;
using KolBlazor.JSInterop;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate;

public sealed partial class MilestoneTimeline : KolComponentBase, IAsyncDisposable
{
    private readonly MilestoneTaskFilter milestoneTaskFilter = [];
    
    [Inject] private KolTimelineJsInterop TimelineInterop { get; set; } = null!;
    
    [CascadingParameter] private IFilterableProjectState State { get; set; } = null!;

    [Parameter] public bool Collapsed { get; set; }

    protected override void OnInitialized()
    {
        // Remove filter if empty
        milestoneTaskFilter.FilterChanged += () =>
        {
            if (milestoneTaskFilter.Count == 0)
            {
                State.TaskFilter.RemoveFilter(milestoneTaskFilter);
            }
            else if (!State.TaskFilter.Filters.Contains(milestoneTaskFilter))
            {
                State.TaskFilter.AddFilter(milestoneTaskFilter);
            }
        };

        if (State is IRefreshableProjectState refreshableProjectState)
        {
            refreshableProjectState.RefreshService.Refreshed += OnRefresh;
        }

        State.MilestonesChanged += _ => StateHasChanged();
        State.TasksChanged += _ => StateHasChanged();
    }

    /// <summary>
    /// Refreshes the milestone task filter based on the current state.
    /// If the filter is not empty, it removes any milestones that are no longer present in the state.
    /// </summary>
    private void OnRefresh()
    {
        if (milestoneTaskFilter.Count <= 0) return;
        var deletedMilestones = milestoneTaskFilter.Except(State.Milestones);
        milestoneTaskFilter.RemoveRange(deletedMilestones);
    }

    private IEnumerable<(Milestone milestone, DateTime start)> TimelineMilestones
    {
        get
        {
            var beginOfYear = new DateTime(DateTime.Now.Year, 1, 1);
            var endOfYear = new DateTime(DateTime.Now.Year, 12, 31);

            return State.Milestones
                .Select(x => (Milestone: x, Start: State.GetMilestoneStart(x)))
                .Where(d => beginOfYear <= d.Start && d.Start <= endOfYear)
                .OrderBy(x => x.Start);
        }
    }

    /// <summary>
    /// Disposes the asynchronous operation.
    /// </summary>
    /// <returns>A ValueTask representing the asynchronous operation.</returns>
    public ValueTask DisposeAsync()
    {
        if (State is IRefreshableProjectState refreshableProjectState)
        {
            refreshableProjectState.RefreshService.Refreshed -= OnRefresh;
        }

        return TimelineInterop.DisposeAsync();
    }
}

using Cardmngr.Components.ProjectAggregate.States;
using Cardmngr.Shared.Utils.Filter.TaskFilters;
using KolBlazor;
using KolBlazor.JSInterop;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate;

public partial class MilestoneTimeline : KolComponentBase, IAsyncDisposable
{
    
    [CascadingParameter] IFilterableProjectState State { get; set; } = null!;
    private readonly MilestoneTaskFilter milestoneTaskFilter = [];

    [Parameter]
    public bool Collapsed { get; set; }

    [Inject] KolTimelineJsInterop TimelineInterop { get; set; } = null!;

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

        if (State is IRefresheableProjectState refresheableProjectState)
        {
            refresheableProjectState.RefreshService.Refreshed += OnRefresh;
        }

        State.MilestonesChanged += StateHasChanged;
        State.TasksChanged += _ => StateHasChanged();
    }

    /// <summary>
    /// Refreshes the milestone task filter based on the current state.
    /// If the filter is not empty, it removes any milestones that are no longer present in the state.
    /// </summary>
    private void OnRefresh()
    {
        if (milestoneTaskFilter.Count > 0)
        {
            var deletedMilestones = milestoneTaskFilter.Except(State.Milestones);
            milestoneTaskFilter.RemoveRange(deletedMilestones);
        }
    }

    /// <summary>
    /// Disposes the asynchronous operation.
    /// </summary>
    /// <returns>A ValueTask representing the asynchronous operation.</returns>
    public ValueTask DisposeAsync()
    {
        if (State is IRefresheableProjectState refresheableProjectState)
        {
            refresheableProjectState.RefreshService.Refreshed -= OnRefresh;
        }

        return TimelineInterop.DisposeAsync();
    }
}

using Blazored.Modal;
using Blazored.Modal.Services;
using Cardmngr.Components.MilestoneAggregate.Modals;
using Cardmngr.Components.ProjectAggregate.Modals;
using Cardmngr.Components.ProjectAggregate.States;
using Cardmngr.Components.TaskAggregate.Modals;
using Cardmngr.Domain.Entities;
using Cardmngr.Utils;
using KolBlazor.Components.Charts.Data;
using KolBlazor.Components.Charts.Gantt;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate;

public partial class ProjectGantt : ComponentBase
{
    private GanttChart _chart = null!;
    private GanttDetalizationLevel _detalizationLevel = GanttDetalizationLevel.Quarter;

    [CascadingParameter] private IProjectState? State { get; set; }
    [CascadingParameter] private IProjectStateFinder? StateFinder { get; set; }

    [CascadingParameter(Name = "DetailsModal")]
    private ModalOptions DetailsModal { get; set; } = null!;

    [CascadingParameter] private IModalService Modal { get; set; } = null!;

    [Parameter] public Func<IEnumerable<GanttChartItem>> GetItems { get; set; } = () => [];
    [Parameter] public bool Multiple { get; set; }
    [Parameter] public IEnumerable<OnlyofficeTaskStatus> Statuses { get; set; } = null!;
    [Parameter] public bool HighlightRoot { get; set; }
    [Parameter] public EventCallback<GanttChartItem> ItemExpandToggled { get; set; }

    protected override void OnInitialized()
    {
        if (Multiple)
        {
            if (StateFinder is null)
            {
                throw new InvalidOperationException("StateFinder is not set");
            }
        }
        else
        {
            if (State is null)
            {
                throw new InvalidOperationException("State is not set");
            }

            State.TasksChanged += _ => Refresh();
            State.MilestonesChanged += _ => Refresh();

            if (State is IFilterableProjectState filterableState)
            {
                filterableState.TaskFilter.FilterChanged += Refresh;
            }
        }
    }

    public void Refresh()
    {
        _chart.RefreshItems();
        _chart.Refresh();
        InvokeAsync(StateHasChanged);
    }

    private void ScrollToToday()
    {
        _chart.ScrollTo(DateTime.Today);
    }

    private void OnDetalizationLevelChanged(GanttDetalizationLevel level)
    {
        _detalizationLevel = level;
        _chart.Refresh();
    }

    private static string GetDetalizationLevelText(GanttDetalizationLevel level)
    {
        return level switch
        {
            GanttDetalizationLevel.Week => "Неделя",
            GanttDetalizationLevel.Month => "Месяц",
            GanttDetalizationLevel.Quarter => "Квартал",
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };
    }

    private static string GetItemKey(GanttChartItem item)
    {
        return item.Data switch
        {
            OnlyofficeTask task => $"task-{task.Id}",
            Milestone milestone => $"milestone-{milestone.Id}",
            IProjectState state => $"project-{state.Project.Id}",
            _ => throw new NotSupportedException()
        };
    }

    private Task OnItemClicked(GanttChartItem item)
    {
        return item.Data switch
        {
            OnlyofficeTask task => OpenDetailsTaskModal(task),
            IProjectState state => OpenDetailsProjectModal(state),
            Milestone milestone => OpenDetailsMilestoneModal(milestone),
            _ => Task.CompletedTask 
        };
    }

    private async Task OpenDetailsTaskModal(OnlyofficeTask task)
    {
        await Modal.Show<TaskDetailsModal>(
            new ModalParameters
            {
                { "Model", task },
                { "State", Multiple ? StateFinder!.Find(task) : State },
                { "TaskTags", task.Tags }
            },
            DetailsModal)
        .Result;
    }

    private async Task OpenDetailsMilestoneModal(Milestone milestone)
    {
        await Modal.Show<MilestoneDetailsModal>(
            new ModalParameters
            {
                { "Model", milestone },
                { "State", Multiple ? StateFinder!.Find(milestone) : State }
            }, 
            DetailsModal)
        .Result;
    }

    private Task<ModalResult> OpenDetailsProjectModal(IProjectState state)
    {
        return Modal.Show<ProjectDetailsModal>(
            new ModalParameters { { "State", state } },
            DetailsModal)
        .Result;
    }


}

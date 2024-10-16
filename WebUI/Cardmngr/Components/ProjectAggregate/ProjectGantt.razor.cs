using Cardmngr.Components.ProjectAggregate.States;
using Cardmngr.Domain.Entities;
using KolBlazor.Components.Charts.Data;
using KolBlazor.Components.Charts.Gantt;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate;

public partial class ProjectGantt : ComponentBase
{
    private GanttChart _chart = null!;
    private GanttDetalizationLevel _detalizationLevel = GanttDetalizationLevel.Quarter;

    [CascadingParameter] private IProjectState State { get; set; } = null!;

    [Parameter] public Func<IEnumerable<GanttChartItem>> GetItems { get; set; } = () => [];

    protected override void OnInitialized()
    {
        if (State is not null)
        {
            State.TasksChanged += _ => Refresh();

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
            Project project => $"project-{project.Id}",
            ProjectInfo projectInfo => $"projectInfo-{projectInfo.Id}",
            MilestoneInfo milestoneInfo => $"milestoneInfo-{milestoneInfo.Id}",
            _ => throw new NotSupportedException()
        };
    }
}

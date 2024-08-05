using Cardmngr.Components.ProjectAggregate.States;
using Cardmngr.Domain.Entities;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate.Dashboard;

public partial class DashboardCardsColumn : ComponentBase
{
    private IList<OnlyofficeTask> _tasks = null!;
    [CascadingParameter] IFilterableProjectState State { get; set; } = null!;

    [Parameter, EditorRequired] public Func<IFilterableProjectState, IList<OnlyofficeTask>> TaskRefreshFunc { get; set; } = null!;
    [Parameter, EditorRequired] public string Header { get; set; } = null!;

    [Parameter] public Color Color { get; set; }

    protected override void OnInitialized()
    {
        _tasks = TaskRefreshFunc(State);
        State.TasksChanged += _ => RefreshTasks();
        State.TaskFilter.FilterChanged += RefreshTasks;
    }

    private void RefreshTasks()
    {
        _tasks = TaskRefreshFunc(State);
        StateHasChanged();
    }
}

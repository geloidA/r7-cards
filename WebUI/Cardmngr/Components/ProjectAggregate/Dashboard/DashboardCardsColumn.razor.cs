using Cardmngr.Components.ProjectAggregate.Models;
using Cardmngr.Components.ProjectAggregate.States;
using Cardmngr.Domain.Entities;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate.Dashboard;

public partial class DashboardCardsColumn : ComponentBase
{
    private IList<OnlyofficeTask> _tasks = null!;
    [CascadingParameter] IProjectState State { get; set; } = null!;

    [Parameter, EditorRequired] public Func<IProjectState, IList<OnlyofficeTask>> TaskRefreshFunc { get; set; } = null!;
    [Parameter, EditorRequired] public string Header { get; set; } = null!;

    [Parameter] public Color Color { get; set; }

    protected override void OnInitialized()
    {
        _tasks = TaskRefreshFunc(State);
        State.TasksChanged += RefreshTasks;
    }

    private void RefreshTasks(TaskChangedEventArgs? e)
    {
        _tasks = TaskRefreshFunc(State);
        StateHasChanged();
    }
}

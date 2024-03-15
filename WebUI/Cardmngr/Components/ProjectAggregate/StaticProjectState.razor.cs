using Cardmngr.Domain.Entities;
using Cardmngr.Pages;
using Cardmngr.Shared;
using Cardmngr.Shared.Project;
using Cardmngr.Shared.Utils.Filter;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate;

public sealed partial class StaticProjectState : ProjectStateBase, IProjectState, IDisposable // TODO: Rename
{
    private bool isCollapsed = true;

    private string CssHeight => isCollapsed 
        ? "min-height: 50px;" 
        : "max-height: 650px; min-height: 650px;";

    [Parameter] public IProjectStateVm? ViewModel { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [CascadingParameter(Name = "UserId")] string? UserId { get; set; }
    [CascadingParameter] SelfTasksPage? SelfTasksPage { get; set; }

    private bool IsCurrentUserInTeam => Model is { } && Model.Team.Any(x => x.Id == UserId);

    private readonly TaskFilterManager taskFilterManager = new();
    public override IFilterManager<OnlyofficeTask> TaskFilter => taskFilterManager;

    protected override void OnInitialized()
    {
        if (SelfTasksPage is { })
        {
            SelfTasksPage.CollapseChanged += ChangeCollapse;
        }
    }

    protected override void OnParametersSet()
    {
        if (ViewModel is { })
        {
            Model = ViewModel;
            Initialized = true;
        }
    }

    private void ToggleCollapsed()
    {
        isCollapsed = !isCollapsed;
    }

    private void ChangeCollapse(bool value) => isCollapsed = value;

    public void Dispose()
    {
        if (SelfTasksPage is { })
        {
            SelfTasksPage.CollapseChanged -= ChangeCollapse;
        }
    }
}

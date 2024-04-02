using Cardmngr.Components.ProjectAggregate.Vms;
using Cardmngr.Domain.Entities;
using Cardmngr.Shared;
using Cardmngr.Shared.Utils.Filter;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate;

public sealed partial class StaticProjectState : ProjectStateBase, IProjectState // TODO: Rename
{
    private string CssHeight => ViewModel?.IsCollapsed ?? true 
        ? "min-height: 50px;" 
        : "max-height: 650px; min-height: 650px;";

    [Parameter] public StaticProjectVm? ViewModel { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [CascadingParameter(Name = "UserId")] string? UserId { get; set; }

    private bool IsCurrentUserInTeam => Model is { } && Model.Team.Any(x => x.Id == UserId);

    private readonly TaskFilterManager taskFilterManager = new();
    public override IFilterManager<OnlyofficeTask> TaskFilter => taskFilterManager;

    protected override void OnParametersSet()
    {
        if (ViewModel is { })
        {
            Model = ViewModel.StateVm;
            Initialized = true;
        }
    }

    private void ToggleCollapsed()
    {
        if (ViewModel is { })
        {
            ViewModel.IsCollapsed = !ViewModel.IsCollapsed;
        }
    }
}

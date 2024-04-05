using Cardmngr.Application.Clients;
using Cardmngr.Components.ProjectAggregate.Vms;
using Cardmngr.Domain.Entities;
using Cardmngr.Services;
using Cardmngr.Shared;
using Cardmngr.Shared.Utils.Filter;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate;

public sealed partial class StaticProjectState : ProjectStateBase, IProjectState, IDisposable // TODO: Rename
{
    private string CssHeight => ViewModel?.IsCollapsed ?? true 
        ? "min-height: 50px;" 
        : "max-height: 650px; min-height: 650px;";

    private Icon CollapsedIcon => ViewModel?.IsCollapsed ?? true 
        ? new Icons.Regular.Size16.ChevronDown() 
        : new Icons.Regular.Size16.ChevronUp();

    [Parameter] public StaticProjectVm? ViewModel { get; set; }
    [Inject] IProjectClient ProjectClient { get; set; } = null!;
    [Inject] AllProjectsPageSummaryService SummaryService { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        if (ViewModel is { IsCollapsed: false })
        {
            Model = await ProjectClient.CreateProjectWithTasksAsync(ViewModel.Tasks);
            Initialized = true;
        }

        SummaryService.OnProjectsChanged += ToggleIfSelected;
    }

    private async void ToggleIfSelected()
    {
        await Task.Delay(5); // wait for correct toggling
        if (ViewModel is { } && SummaryService.FilterManager.ProjectId == ViewModel.ProjectInfo.Id)
        {
            await ToggleCollapsed();
            StateHasChanged();
        }
    }

    private readonly TaskFilterManager taskFilterManager = new();
    public override IFilterManager<OnlyofficeTask> TaskFilter => taskFilterManager;

    private async Task ToggleCollapsed()
    {
        if (ViewModel is { })
        {
            ViewModel.IsCollapsed = !ViewModel.IsCollapsed;

            Model = ViewModel.IsCollapsed ? null : await ProjectClient.CreateProjectWithTasksAsync(ViewModel.Tasks);
            Initialized = Model is { };
        }
    }

    public void Dispose()
    {
        SummaryService.OnProjectsChanged -= ToggleIfSelected;
    }
}

using Cardmngr.Domain;
using Cardmngr.Domain.Entities;
using Cardmngr.Services;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.Header;

public partial class FilterCustomizer : ComponentBase
{
    private bool show;
    private bool onlyDeadlined;
    private bool onlyClosed;

    [Inject] AllProjectsPageSummaryService SummaryService { get; set; } = null!;
    [Inject] NavigationManager NavigationManager { get; set; } = null!;

    protected override void OnInitialized()
    {        
        SummaryService.OnProjectsChanged += () =>
        {
            selectedResponsible ??= SummaryService
                .GetResponsibles()
                .SingleOrDefault(x => x.Id == SummaryService.FilterManager.Responsible);

            StateHasChanged();
        };
        show = IsFilterPage(NavigationManager.Uri);
        NavigationManager.LocationChanged += (_, args) => UpdateStateByLocation(args.Location);
    }

    private UserInfo? selectedResponsible;

    private void SelectResponsible(UserInfo? user)
    {
        if (selectedResponsible?.Id == user?.Id) return;
        SummaryService.FilterManager.Responsible = user?.Id;
        selectedResponsible = user;
    }

    private UserInfo? selectedCreatedBy; // TODO: DRY

    private void SelectCreatedBy(UserInfo? user)
    {
        if (selectedCreatedBy?.Id == user?.Id) return;
        SummaryService.FilterManager.CreatedBy = user?.Id;
        selectedCreatedBy = user;
    }

    private ProjectInfo? selectedProject;

    private void SelectProject(ProjectInfo? project)
    {
        if (selectedProject?.Id == project?.Id) return;
        SummaryService.FilterManager.ProjectId = project?.Id;
        selectedProject = project;
    }

    private void ToggleDeadlineFilter()
    {
        onlyDeadlined = SummaryService.FilterManager.ToggleDeadlineFilter();
    }

    private void ToggleClosedFilter()
    {
        onlyClosed = SummaryService.FilterManager.ToggleClosedFilter();
    }

    private static bool IsFilterPage(string uri) => uri.EndsWith("/all-projects", StringComparison.OrdinalIgnoreCase);

    private void UpdateStateByLocation(string uri)
    {
        show = IsFilterPage(uri);

        StateHasChanged();
    }
}
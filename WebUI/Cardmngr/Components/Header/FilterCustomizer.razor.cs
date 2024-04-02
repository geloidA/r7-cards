using Cardmngr.Domain.Entities;
using Cardmngr.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Onlyoffice.Api.Providers;

namespace Cardmngr.Components.Header;

public partial class FilterCustomizer : ComponentBase
{
    private bool show;
    private bool isFirstVisit = true;
    private bool onlyDeadlined;
    private bool onlyClosed;

    [Inject] AllProjectsPageSummaryService SummaryService { get; set; } = null!;
    [Inject] NavigationManager NavigationManager { get; set; } = null!;
    [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

    protected override void OnInitialized()
    {
        SummaryService.OnProjectsChanged += OnProjectsChanged;
        show = IsFilterPage(NavigationManager.Uri);
        NavigationManager.LocationChanged += (_, args) => UpdateStateByLocation(args.Location);
    }

    private void OnProjectsChanged()
    {
        if (isFirstVisit)
        {
            isFirstVisit = false;
            selectedResponsible = SummaryService
                .GetResponsibles()
                .SingleOrDefault(x => x.Id == AuthenticationStateProvider.ToCookieProvider().UserId); 
        }

        StateHasChanged();
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
        SummaryService.FilterManager.SetCreatedBy(user?.Id);
        selectedCreatedBy = user;
    }

    private Project? selectedProject;

    private void SelectProject(Project? project)
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

        if (!show)
        {
            Cleanup();
        }

        StateHasChanged();
    }

    private void Cleanup()
    {
        onlyClosed = false;
        isFirstVisit = true;
        onlyDeadlined = false;
        selectedCreatedBy = null;
        selectedResponsible = null;
        selectedProject = null;
    }
}

using System.Text.Json;
using Cardmngr.Application.Clients;
using Cardmngr.Application.Clients.People;
using Cardmngr.Domain.Entities;
using Cardmngr.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Onlyoffice.Api.Models;
using Onlyoffice.Api.Providers;

namespace Cardmngr.Components.Header;

public partial class FilterCustomizer : ComponentBase
{
    private bool show;
    private bool onlyDeadlined;
    private bool onlyClosed;

    private IEnumerable<UserInfo> users = null!;
    private IEnumerable<Project> projects = null!;

    [Inject] FilterManagerService FilterManager { get; set; } = null!;
    [Inject] NavigationManager NavigationManager { get; set; } = null!;
    [Inject] IPeopleClient PeopleClient { get; set; } = null!;
    [Inject] IProjectClient ProjectClient { get; set; } = null!;
    [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

    protected async override Task OnInitializedAsync() // TODO: clean up
    {
        var department = JsonSerializer.Deserialize<UserProfileDto>(AuthenticationStateProvider.ToCookieProvider()["Data"])?.Department;
        users = await PeopleClient.GetUsersAsync().Where(x => x.Department == department).ToListAsync();
        projects = await ProjectClient.GetProjectsAsync().ToListAsync();

        show = IsFilterPage(NavigationManager.Uri);
        NavigationManager.LocationChanged += (_, args) => UpdateStateByLocation(args.Location);
    }

    private UserInfo? selectedResponsible;

    private void SelectResponsible(UserInfo? user)
    {
        Console.WriteLine($"Responsible changed on {SummaryService.FilterManager.Responsible}");
        Console.WriteLine($"Selected responsible is {selectedResponsible?.Id ?? "null"}");
        if (SummaryService.FilterManager.Responsible != selectedResponsible?.Id)
        {
            selectedResponsible = SummaryService
                .GetResponsibles()
                .SingleOrDefault(x => x.Id == SummaryService.FilterManager.Responsible);
            StateHasChanged();
        }
    }

    private UserInfo? selectedCreatedBy; // TODO: DRY

    private void SelectCreatedBy(UserInfo? user)
    {
        if (selectedCreatedBy?.Id == user?.Id) return;
        FilterManager.SetCreatedBy(user?.Id);
        selectedCreatedBy = user;
    }

    private Project? selectedProject;

    private void SelectProject(Project? project)
    {
        if (selectedProject?.Id == project?.Id) return;
        FilterManager.SetProjectId(project?.Id);
        selectedProject = project;
    }

    private void ToggleDeadlineFilter()
    {
        onlyDeadlined = FilterManager.ToggleDeadlineFilter();
    }

    private void ToggleClosedFilter()
    {
        onlyClosed = FilterManager.ToggleClosedFilter();
    }

    private static bool IsFilterPage(string uri) => uri.EndsWith("/self-tasks");

    private void UpdateStateByLocation(string uri)
    {
        show = IsFilterPage(uri);
        StateHasChanged();
    }
}

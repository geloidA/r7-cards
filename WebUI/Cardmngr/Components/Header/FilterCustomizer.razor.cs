using Cardmngr.Domain;
using Cardmngr.Domain.Entities;
using Cardmngr.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

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
            if (!SelectedResponsible.Any())
            {
                var responsible = SummaryService
                    .GetResponsibles()
                    .SingleOrDefault(x => x.Id == SummaryService.FilterManager.Responsible);
                if (responsible is { })
                {
                    SelectedResponsible = [responsible];
                }
            }

            StateHasChanged();
        };
        show = IsFilterPage(NavigationManager.Uri);
        NavigationManager.LocationChanged += (_, args) => UpdateStateByLocation(args.Location);
    }

    private IEnumerable<ProjectInfo> SelectedProject = [];

    private void SelectionProjectChanged(IEnumerable<ProjectInfo> projects)
    {
        var selectedProject = projects.SingleOrDefault();
        SummaryService.FilterManager.ProjectId = selectedProject?.Id;

        SelectedProject = selectedProject is { } ? [selectedProject] : [];
    }

    private void OnProjectSearch(OptionsSearchEventArgs<ProjectInfo> e)
    {
        e.Items = SummaryService
            .GetProjects()
            .Where(x => x.Title.StartsWith(e.Text, StringComparison.CurrentCultureIgnoreCase))
            .OrderBy(x => x.Title);
    }

    private IEnumerable<UserInfo> SelectedResponsible = [];

    private void SelectionResponsibleChanged(IEnumerable<UserInfo> users)
    {
        var selectedResponsible = users.SingleOrDefault();
        SummaryService.FilterManager.Responsible = selectedResponsible?.Id;

        SelectedResponsible = selectedResponsible is { } ? [selectedResponsible] : [];
    }

    private void OnResponsibleSearch(OptionsSearchEventArgs<UserInfo> e)
    {
        e.Items = SummaryService
            .GetResponsibles()
            .Where(x => x.DisplayName.StartsWith(e.Text, StringComparison.CurrentCultureIgnoreCase))
            .OrderBy(x => x.DisplayName);
    }

    private IEnumerable<UserInfo> SelectedCreatedBy = [];

    private void SelectionCreatedByChanged(IEnumerable<UserInfo> users)
    {
        var selectedCreatedBy = users.SingleOrDefault();
        SummaryService.FilterManager.CreatedBy = selectedCreatedBy?.Id;

        SelectedCreatedBy = selectedCreatedBy is { } ? [selectedCreatedBy] : [];
    }

    private void OnCreatedBySearch(OptionsSearchEventArgs<UserInfo> e)
    {
        e.Items = SummaryService
            .GetCreatedBys()
            .Where(x => x.DisplayName.StartsWith(e.Text, StringComparison.CurrentCultureIgnoreCase))
            .OrderBy(x => x.DisplayName);
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
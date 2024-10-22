using Cardmngr.Domain.Entities;
using Cardmngr.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Cardmngr.Components.Header;

public partial class FilterCustomizer : ComponentBase, IDisposable
{
    private bool show;
    private readonly TaskSelectorType[] _taskSelectorTypes = Enum.GetValues<TaskSelectorType>();

    [Inject] AllProjectsPageSummaryService SummaryService { get; set; } = null!;
    [Inject] NavigationManager NavigationManager { get; set; } = null!;
    [Inject] IProjectFollowChecker ProjectFollowChecker { get; set; } = null!;

    private Color GanttModeColor => SummaryService.GanttModeEnabled ? Color.Accent : Color.Disabled;

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
        NavigationManager.LocationChanged += UpdateStateByLocation;
    }

    private void UpdateStateByLocation(object? sender, LocationChangedEventArgs args)
    {
        show = IsFilterPage(args.Location);

        StateHasChanged();
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
            .Where(x => x.Title.Contains(e.Text, StringComparison.CurrentCultureIgnoreCase))
            .OrderByDescending(x => ProjectFollowChecker.IsFollow(x.Id));
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
            .Where(x => x.DisplayName.Contains(e.Text, StringComparison.CurrentCultureIgnoreCase))
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
            .Where(x => x.DisplayName.Contains(e.Text, StringComparison.CurrentCultureIgnoreCase))
            .OrderBy(x => x.DisplayName);
    }

    private static bool IsFilterPage(string uri) => uri.EndsWith("/all-projects", StringComparison.OrdinalIgnoreCase);

    public void Dispose()
    {
        NavigationManager.LocationChanged -= UpdateStateByLocation;
    }
}
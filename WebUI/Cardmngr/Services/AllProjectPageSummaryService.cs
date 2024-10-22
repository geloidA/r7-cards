using BlazorComponentBus;
using Cardmngr.Domain.Entities;
using Cardmngr.Pages.Contracts;

namespace Cardmngr.Services;

public class AllProjectsPageSummaryService(ComponentBus bus)
{
    private List<OnlyofficeTask>? _tasks = [];

    public FilterManagerService FilterManager { get; } = new();

    public event Action? OnProjectsChanged;

    public void SetTasks(List<OnlyofficeTask> tasks, bool notify = true)
    {
        _tasks = tasks;
        
        if (notify)
        {
            OnProjectsChanged?.Invoke();
        }
    }

    public bool GanttModeEnabled { get; private set; }

    public void ToggleGanttMode()
    {
        GanttModeEnabled = !GanttModeEnabled;
        bus.Publish(new GanttModeToggled());
    }

    public IEnumerable<UserInfo> GetResponsibles() => _tasks?
        .SelectMany(x => x.Responsibles)
        .Distinct() ?? [];

    public IEnumerable<ProjectInfo> GetProjects() => _tasks?
        .Select(x => x.ProjectOwner)
        .Distinct() ?? [];

    public IEnumerable<UserInfo> GetCreatedBys() => _tasks?
        .Select(x => x.CreatedBy)
        .Distinct() ?? [];
    
    public void LeftPage() => _tasks = null;
}

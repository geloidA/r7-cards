using Cardmngr.Domain;
using Cardmngr.Domain.Entities;

namespace Cardmngr.Services;

public class AllProjectsPageSummaryService
{
    private List<OnlyofficeTask>? tasks = [];

    public FilterManagerService FilterManager { get; } = new();

    public event Action? OnProjectsChanged;

    public void SetTasks(List<OnlyofficeTask> tasks, bool notify = true)
    {
        this.tasks = tasks;
        
        if (notify)
        {
            OnProjectsChanged?.Invoke();
        }
    }

    public IEnumerable<UserInfo> GetResponsibles() => tasks?
        .SelectMany(x => x.Responsibles)
        .Distinct() ?? [];

    public IEnumerable<ProjectInfo> GetProjects() => tasks?
        .Select(x => x.ProjectOwner)
        .Distinct() ?? [];

    public IEnumerable<UserInfo> GetCreatedBys() => tasks?
        .Select(x => x.CreatedBy)
        .Distinct() ?? [];
    
    public void LeftPage() => tasks = null;
}

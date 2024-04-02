using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Project;

namespace Cardmngr.Services;

public class AllProjectsPageSummaryService
{
    private List<IProjectStateVm>? projects = [];
    
    public FilterManagerService FilterManager { get; private set; } = new();

    public event Action? OnProjectsChanged;

    public void SetProjects(List<IProjectStateVm> projects)
    {
        this.projects = projects;
        OnProjectsChanged?.Invoke();
    }

    public IEnumerable<UserInfo> GetResponsibles() => projects?
        .SelectMany(x => x.Tasks.SelectMany(y => y.Responsibles))
        .Distinct() ?? [];

    public IEnumerable<Project> GetProjects() => projects?
        .Where(x => x.Project != null)
        .Select(x => x.Project!) ?? [];

    public IEnumerable<UserInfo> GetCreatedBys() => projects?
        .SelectMany(x => x.Tasks.Select(y => y.CreatedBy))
        .Distinct() ?? [];
    
    public void LeftPage()
    {
        projects = null;
        FilterManager = new();
    }
}

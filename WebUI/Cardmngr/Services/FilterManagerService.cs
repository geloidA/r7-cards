using Onlyoffice.Api.Common;

namespace Cardmngr.Services;

public class FilterManagerService
{
    private bool onlyClosed;
    private bool onlyDeadlined;

    public event Action<TaskFilterBuilder>? FilterChanged;
    private void OnFilterChanged() => FilterChanged?.Invoke(GenerateFilter());

    public TaskFilterBuilder GenerateFilter()
    {
        var builder = TaskFilterBuilder.Instance
            .Creator(withCreatedBy)
            .Participant(withResponsible);

        if (onlyClosed) builder = builder.Status(Status.Closed);
        builder = projectId != null ? builder.ProjectId(projectId.Value) : builder.MyProjects(true);

        return onlyDeadlined
            ? builder
                .DeadlineStop(DateTime.Now)
                .Status(Status.Open)
            : builder;
    }

    public bool ToggleClosedFilter()
    {
        onlyClosed = !onlyClosed;
        OnFilterChanged();
        return onlyClosed;
    }

    public bool ToggleDeadlineFilter()
    {
        onlyDeadlined = !onlyDeadlined;
        OnFilterChanged();
        return onlyDeadlined;
    }

    private string? withResponsible;
    public string? Responsible 
    {
        get => withResponsible;
        set
        {
            withResponsible = value;
            OnFilterChanged();
        }
    }

    private string? withCreatedBy;
    public string? CreatedBy
    {
        get => withCreatedBy;
        set
        {
            withCreatedBy = value;
            OnFilterChanged();
        }
    }

    private int? projectId;
    public int? ProjectId 
    {
        get => projectId;
        set
        {
            projectId = value;
            OnFilterChanged();
        }
    }
}

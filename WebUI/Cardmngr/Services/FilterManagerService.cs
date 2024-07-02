using System.ComponentModel;
using Cardmngr.Extensions;
using Onlyoffice.Api.Models.Common;

namespace Cardmngr.Services;

public class FilterManagerService
{
    public event Action<TaskFilterBuilder>? FilterChanged;
    private void OnFilterChanged() => FilterChanged?.Invoke(GenerateFilter());

    public TaskFilterBuilder GenerateFilter()
    {
        var builder = TaskFilterBuilder.Instance
            .Creator(withCreatedBy)
            .Participant(withResponsible);

        builder = projectId != null ? builder.ProjectId(projectId.Value) : builder.MyProjects(true);

        return taskSelectorType switch 
        {
            TaskSelectorType.InProgress => builder.Status(Status.Open),
            TaskSelectorType.Deadlined => builder.DeadlineOutside(),
            TaskSelectorType.Closed => builder.Status(Status.Closed),
            _ => builder
        };
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

    private TaskSelectorType taskSelectorType;
    public TaskSelectorType TaskSelectorType
    {
        get => taskSelectorType;
        set
        {
            taskSelectorType = value;
            OnFilterChanged();
        }
    }
}

public enum TaskSelectorType
{
    [Description("Все")]
    None,
    [Description("Закрытые")]
    Closed,
    [Description("Просроченные")]
    Deadlined,
    [Description("Активные")]
    InProgress
}
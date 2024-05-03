using System.ComponentModel;
using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Utils;

namespace Cardmngr.Report;

public class ProjectTaskReportRequest
{
    public IEnumerable<Project> Projects { get; set; } = [];
    public IEnumerable<UserInfo> Responsibles { get; set; } = [];
    public IEnumerable<UserInfo> Creators { get; set; } = [];
    public DateRange StartDateRange { get; } = new();
    public DateRange DeadlineRange { get; } = new();
    public bool OnlyDeadline { get; set; }
    public TaskStatusType TaskStatusType { get; set; }
}

public enum TaskStatusType
{
    [Description("Все")]
    None,
    [Description("Открытые")]
    Open,
    [Description("Закрытые")]
    Closed
}

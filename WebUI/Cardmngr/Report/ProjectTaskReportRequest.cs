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
}

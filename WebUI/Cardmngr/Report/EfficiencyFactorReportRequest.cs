using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Utils;

namespace Cardmngr.Reports;

public class EfficiencyFactorReportRequest
{
    public IEnumerable<Project> Projects { get; set; } = [];
    public IEnumerable<UserInfo> Responsibles { get; set; } = [];
    public DateRange Deadline { get; set; } = new();

    public IEnumerable<Group> Group { get; set; } = [];
}

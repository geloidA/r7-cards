using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Utils;

namespace Cardmngr.Reports;

public class CreatedTaskReportRequest
{
    public IEnumerable<Project> Projects { get; set; } = [];
    public IEnumerable<UserInfo> CreatedBys { get; set; } = [];
    public DateRange CreatedByRange { get; set; } = new();
    public bool SelfDepartment { get; set; }
}

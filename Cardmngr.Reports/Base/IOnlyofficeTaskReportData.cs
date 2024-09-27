namespace Cardmngr.Reports.Base;

public interface IOnlyofficeTaskReportData
{
    string Title { get; }
    string Responsibles { get; }
    string Deadline { get; }
    string StatusString { get; }
    bool IsDeadlineOverdue { get; }
}

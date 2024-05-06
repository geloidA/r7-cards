namespace Cardmngr.Reports;

public interface IOnlyofficeTaskReportData
{
    string Title { get; }
    string Responsibles { get; }
    string Deadline { get; }
    string StatusString { get; }
    bool IsDeadlineOverdue { get; }
}

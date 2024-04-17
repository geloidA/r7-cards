namespace Cardmngr.Reports;

public interface IOnlyofficeTaskReportData
{
    string Title { get; set; }
    string Responsibles { get; set; }
    string Deadline { get; set; }
    string StatusString { get; set; }
}

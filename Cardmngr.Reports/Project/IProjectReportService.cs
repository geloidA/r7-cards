namespace Cardmngr.Reports.Project;

public interface IProjectReportService
{
    byte[] GetPdfReport(int projectId);
}

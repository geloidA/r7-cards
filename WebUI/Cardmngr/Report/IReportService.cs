using Cardmngr.Reports.Base;

namespace Cardmngr.Report;

public interface IReportService : IAsyncDisposable
{
    IReportGenerator? Generator { set; }
    Task GenerateReport(string fileName);
}

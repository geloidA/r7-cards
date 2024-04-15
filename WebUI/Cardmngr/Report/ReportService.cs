using Cardmngr.Reports.Base;

namespace Cardmngr.Report;

public abstract class ReportService(IReportGenerator generator, ReportJSModule jsModule)
{
    protected readonly IReportGenerator generator = generator;
    private readonly ReportJSModule jsModule = jsModule;

    /// <summary>
    /// Generates report
    /// </summary>
    /// <param name="fileName">File name without extension</param>
    /// <returns></returns>
    protected async Task GenerateReport(string fileName)
    {
        await jsModule.SaveAsAsync($"{fileName}.xlsx", generator.GenerateReport());
    }
}

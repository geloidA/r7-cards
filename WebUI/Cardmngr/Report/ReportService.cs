using Cardmngr.Reports.Base;

namespace Cardmngr.Report;

public class ReportService(ReportJsModule jsModule) : IReportService
{
    private IReportGenerator? generator;
    private readonly ReportJsModule jsModule = jsModule;

    public IReportGenerator? Generator { set => generator = value; }

    public ValueTask DisposeAsync() => jsModule.DisposeAsync();

    /// <summary>
    /// Generates report
    /// </summary>
    /// <param name="fileName">File name without extension</param>
    /// <returns></returns>
    public async Task GenerateReport(string fileName)
    {
        if (generator is null)
        {
            throw new InvalidOperationException("Report generator is not set");
        }

        await jsModule.SaveAsAsync($"{fileName}.xlsx", await Task.Run(generator.GenerateReport));
    }
}

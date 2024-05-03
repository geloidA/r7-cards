using Cardmngr.Reports.Base;

namespace Cardmngr.Report;

public class ReportService(ReportJSModule jsModule) : IReportService
{
    private IReportGenerator? generator;
    private readonly ReportJSModule jsModule = jsModule;

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

        var bytes = await Task.Run(generator.GenerateReport);
        await jsModule.SaveAsAsync($"{fileName}.xlsx", bytes);
    }
}

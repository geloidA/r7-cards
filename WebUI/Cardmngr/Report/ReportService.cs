using Cardmngr.Reports.Base;

namespace Cardmngr.Report;

public abstract class ReportService(IReportGenerator generator, ReportJSModule jsModule) : IAsyncDisposable
{
    protected readonly IReportGenerator generator = generator;
    private readonly ReportJSModule jsModule = jsModule;

    public ValueTask DisposeAsync()
    {
        return jsModule.DisposeAsync();
    }

    /// <summary>
    /// Generates report
    /// </summary>
    /// <param name="fileName">File name without extension</param>
    /// <returns></returns>
    protected async Task GenerateReport(string fileName)
    {
        var bytes = await Task.Run(() => generator.GenerateReport());
        await jsModule.SaveAsAsync($"{fileName}.xlsx", bytes);
    }
}

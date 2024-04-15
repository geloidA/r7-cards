using Cardmngr.Domain.Entities;
using Cardmngr.Reports;

namespace Cardmngr.Report;

public class TaskReportService(TaskReportGenerator taskGenerator, ReportJSModule jsModule) : ReportService(taskGenerator, jsModule)
{
    public async Task GenerateReport(string fileName, List<OnlyofficeTask> tasks)
    {
        (generator as TaskReportGenerator)!.Tasks = tasks;
        await GenerateReport(fileName);
    }
}
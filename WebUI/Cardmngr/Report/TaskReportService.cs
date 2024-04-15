using Cardmngr.Domain.Entities;
using Cardmngr.Reports;

namespace Cardmngr.Report;

public class TaskReportService(TaskReportGenerator taskGenerator, ReportJSModule jsModule) : ReportService(taskGenerator, jsModule)
{
    public async Task GenerateReport(string fileName, IEnumerable<OnlyofficeTask> tasks, IEnumerable<OnlyofficeTaskStatus> statuses)
    {
        if (generator is TaskReportGenerator taskGenerator)
        {
            taskGenerator.Tasks = tasks;
            taskGenerator.Statuses = statuses;
        }

        await GenerateReport(fileName);
    }
}
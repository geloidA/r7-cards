using Cardmngr.Domain.Entities;
using Cardmngr.Reports.Base;
using Cardmngr.Reports.Extensions;
using ClosedXML.Excel;
using Cardmngr.Shared.Extensions;
using Cardmngr.Domain.Enums;
using Cardmngr.Domain;

namespace Cardmngr.Reports;

public class EfficiencyFactorReport : ReportGeneratorBase
{
    public IEnumerable<OnlyofficeTask> Tasks { get; set; } = [];
    public IEnumerable<OnlyofficeTaskStatus>? Statuses { get; set; }

    public override byte[] GenerateReport()
    {
        using XLWorkbook wb = new();

        ConfigureWorkbook(wb);

        var ws = wb.Worksheets.Add("Созданные задачи");

        ConfigureWorksheet(ws);

        var row = GenerateHeader(ws, "Созданные задачи", 1, 5) + 1;

        foreach (var groupByUser in EfficiencyFactorReportData.Create(Tasks).GroupByUser)
        {
            row = GenerateUserHeader(ws, row, groupByUser.Key, groupByUser.SelectMany(x => x));
            
            foreach (var groupByProject in groupByUser)
            {
                row = GenerateProject(ws, row, groupByProject);
            }
        }

        ws.Columns().AdjustToContents(22.0, 50.0);

        return wb.ToArray();
    }

    private static int GenerateUserHeader(IXLWorksheet ws, int row, UserInfo user, IEnumerable<OnlyofficeTask> tasks)
    {
        ws.Row(row).Style
            .Fill.SetBackgroundColor(XLColor.FromHtml("#999999"))
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
        ws.Cell(row, 1).Style.Font.SetBold();

        ws.Cell(row, 1).Value = $"{user.DisplayName}";
        ws.Cell(row, 2).Value = "Всего:";

        var count = tasks.Count();
        var closed = tasks.Count(x => x.Status == Status.Closed);
        var deadlineOverdue = tasks.Count(x => x.IsDeadlineOut());

        ws.Cell(row, 3).Value = count;
        ws.Cell(row, 4).Value = "Процент выполнения:";
        ws.Cell(row, 5).FormulaA1 = $"{closed} / {count}";
        ws.Cell(row, 5).Style.NumberFormat.SetNumberFormatId((int)XLPredefinedFormat.Number.PercentPrecision2);        
        ws.Cell(row, 6).Value = "Процент просрочки:";
        ws.Cell(row, 7).FormulaA1 = $"{deadlineOverdue} / {count}";
        ws.Cell(row, 7).Style.NumberFormat.SetNumberFormatId((int)XLPredefinedFormat.Number.PercentPrecision2);

        ws.Cell(++row, 1).Value = "Задачи";
        ws.Cell(row, 2).Value = "Статус";
        ws.Cell(row, 3).Value = "Крайний срок";
       
        return row + 1;
    }

    private static int GenerateProject(IXLWorksheet ws, int row, IGrouping<ProjectInfo, OnlyofficeTask> groupByProject)
    {
        ws.Cell(row, 1).Value = $"{groupByProject.Key.Title} ({groupByProject.Count()})";

        row++;

        foreach (var task in groupByProject)
        {
            if (task.IsDeadlineOut())
            {
                ws.Row(row).Style.Fill.SetBackgroundColor(XLColor.LightGray);
            }

            ws.Cell(row, 1).Value = task.IsDeadlineOut() ? $"🔥{task.Title}" : task.Title;
            ws.Cell(row, 2).Value = task.Status.GetDesc();
            ws.Cell(row, 3).Value = task.Deadline;

            row++;
        }

        return row + 1;
    }
}

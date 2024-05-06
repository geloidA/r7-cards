using Cardmngr.Domain;
using Cardmngr.Domain.Entities;
using Cardmngr.Reports.Base;
using Cardmngr.Reports.Extensions;
using ClosedXML.Excel;

namespace Cardmngr.Reports;

public class TaskReportGenerator : ReportGeneratorBase
{
    public IEnumerable<OnlyofficeTask> Tasks { get; set; } = [];
    public IEnumerable<OnlyofficeTaskStatus>? Statuses { get; set; }

    private (int Project, int Milestone) Counter;

    public override byte[] GenerateReport()
    {
        Counter = (0, 0);

        using XLWorkbook wb = new();

        ConfigureWorkbook(wb);

        var ws = wb.Worksheets.Add("Задачи проектов");

        ConfigureWorksheet(ws);

        var row = GenerateHeader(ws, "Задачи проектов");
        
        foreach (var groupedByProject in TaskReportData.Create(Tasks, x => new OnlyofficeTaskReportData(x, Statuses)).GroupedTasks)
        {
            ws.Row(row).Style.Fill.SetBackgroundColor(XLColor.LightGray);
            ws.Cell(row, 1).Style.Font.SetBold();
            
            ws.Cell(row, 1).Value = $"{++Counter.Project}.  {groupedByProject.Key.Title}";

            Counter.Milestone = 0;

            row++;
            foreach (var milestone in groupedByProject.OrderBy(x => x.Key == null))
            {
                GenerageMilestonePart(ws, milestone, ref row);
                ws.Row(++row).Height = 15;
            }

            ws.Row(row).Delete();
        }

        ws.Columns().AdjustToContents();

        return wb.ToArray();
    }

    private void GenerageMilestonePart(IXLWorksheet ws, IGrouping<MilestoneInfo, IOnlyofficeTaskReportData> groupedByMilestone, ref int row)
    {
        ws.Row(row).Style
                .Fill.SetBackgroundColor(XLColor.LightGray)
                .Alignment.SetIndent(1)
                .Font.SetBold();

        ws.Cell(row, 1).Value = groupedByMilestone.Key?.Title is { } 
            ? $"{Counter.Project}.{++Counter.Milestone}.  {groupedByMilestone.Key.Title}"
            : $"{Counter.Project}.{++Counter.Milestone}.  Задачи вне вех";

        row++;
        GenerateTasksHeader(ws, row);
        row++;

        var taskCounter = 1;

        foreach (var task in groupedByMilestone)
        {
            GenerateTaskPart(ws, task, row, taskCounter);
            taskCounter++;
            row++;
        }
    }

    private static void GenerateTasksHeader(IXLWorksheet ws, int row)
    {
        ws.Row(row).Height = 25;

        ws.Row(row).Style
            .Border.SetBottomBorder(XLBorderStyleValues.Thin)
            .Border.SetBottomBorderColor(XLColor.Gray)
            .Font.SetFontColor(XLColor.Gray);

        ws.Cell(row, 1).Style.Alignment.Indent = 3;
        ws.Cell(row, 1).Value = "Задача";
        ws.Cell(row, 2).Value = "Ответственные";
        ws.Cell(row, 3).Value = "Крайний срок";
        ws.Cell(row, 4).Value = "Статус";
    }

    private void GenerateTaskPart(IXLWorksheet ws, IOnlyofficeTaskReportData task, int row, int taskCount)
    {
        ws.Row(row).Style
            .Border.SetBottomBorder(XLBorderStyleValues.Thin)
            .Border.SetBottomBorderColor(XLColor.Gray);

        ws.Cell(row, 1).Style.Alignment.Indent = 3;
        ws.Cell(row, 1).Value = $"{Counter.Project}.{Counter.Milestone}.{taskCount}.  {task.Title}";
        ws.Cell(row, 2).Value = task.Responsibles;
        ws.Cell(row, 2).Style.Alignment.WrapText = true;
        ws.Cell(row, 3).Value = task.Deadline;
        ws.Cell(row, 4).Value = task.StatusString;
    }
}
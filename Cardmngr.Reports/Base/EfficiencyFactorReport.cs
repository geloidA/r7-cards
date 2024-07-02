using Cardmngr.Domain.Entities;
using Cardmngr.Reports.Base;
using Cardmngr.Reports.Extensions;
using ClosedXML.Excel;
using Cardmngr.Shared.Extensions;
using Cardmngr.Domain.Enums;

namespace Cardmngr.Reports;

public class EfficiencyFactorReport : ReportGeneratorBase
{
    public IEnumerable<OnlyofficeTask> Tasks { get; set; } = [];
    public IEnumerable<UserInfo> Users { get; set; } = [];

    public override byte[] GenerateReport()
    {
        using XLWorkbook wb = new();

        ConfigureWorkbook(wb);

        var ws = wb.Worksheets.Add("Эффективность пользователей");

        ConfigureWorksheet(ws);

        var row = GenerateHeader(ws, "Эффективность пользователей", 1, 5) + 1;

        foreach (var groupByUser in EfficiencyFactorReportData.Create(Tasks, Users).GroupByUser)
        {
            row = GenerateUserHeader(ws, row, groupByUser.Key, groupByUser.SelectMany(x => x));
            
            foreach (var groupByProject in groupByUser)
            {
                row = GenerateProject(ws, row, groupByProject);
            }

            row++;
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
        ws.Cell(row, 3).Value = tasks.Count();

        ws.Cell(row, 4).Value = "Выполнено:";
        ws.Cell(row, 5).Value = $"{tasks.Count(x => x.Status == Status.Closed)}";

        ws.Cell(row, 6).Value = "Просрочено:";
        ws.Cell(row, 7).Value = $"{tasks.Count(x => x.IsDeadlineOut())}";
       
        return row + 1;
    }

    private static int GenerateProject(IXLWorksheet ws, int row, IGrouping<ProjectInfo, OnlyofficeTask> groupByProject)
    {
        GenerateProjectHeader(ws, row, groupByProject);

        ws.Cell(++row, 1).Value = "Задачи";
        ws.Cell(row, 1).Style.Alignment.SetIndent(2);

        ws.Cell(row, 2).Value = "Крайний срок";
        ws.Row(row).Height = 35;
        ws.Row(row).Style.Font.SetFontColor(XLColor.Gray);
        ws.Cell(row++, 3).Value = "Статус";

        foreach (var task in groupByProject)
        {
            row = GenerateTaskPart(ws, task, row);
        }

        return row + 1;
    }

    private static void GenerateProjectHeader(IXLWorksheet ws, int row, IGrouping<ProjectInfo, OnlyofficeTask> groupByProject)
    {
        ws.Row(row).Style
            .Border.SetTopBorder(XLBorderStyleValues.Thin)
            .Border.SetTopBorderColor(XLColor.Gray);

        ws.Cell(row, 1).Style.Font.SetBold();
        ws.Cell(row, 1).Value = $"{groupByProject.Key.Title}";
        ws.Cell(row, 1).Style.Alignment.SetIndent(1);
        ws.Cell(row, 1).Style.Alignment.SetWrapText(true);
        
        ws.Cell(row, 2).Value = "Всего:";
        ws.Cell(row, 3).Value = $"{groupByProject.Count()}";

        ws.Cell(row, 4).Value = "Выполнено:";
        ws.Cell(row, 5).Value = $"{groupByProject.Count(x => x.Status == Status.Closed)}";

        ws.Cell(row, 6).Value = "Просрочено:";
        ws.Cell(row, 7).Value = $"{groupByProject.Count(x => x.IsDeadlineOut())}";
    }

    private static int GenerateTaskPart(IXLWorksheet ws, OnlyofficeTask task, int row)
    {
        ws.Cell(row, 1).Style
                .Alignment.SetIndent(3)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

        ws.Cell(row, 1).Value = task.IsDeadlineOut() ? $"{task.Title}" : task.Title;

        ws.Cell(row, 1).Style.Alignment.SetWrapText(true);

        var deadlineString = $"{task.Deadline:dd.MM.yyyy}";
        ws.Cell(row, 2).Value = string.IsNullOrEmpty(deadlineString) ? "—" : deadlineString;
        if (task.IsDeadlineOut())
        {
            ws.Cell(row, 2).Style
                .Fill.SetBackgroundColor(XLColor.FromHtml("#cd6968"));
        }
        ws.Cell(row, 3).Value = task.Status.GetDesc();

        if (task.Subtasks.Count != 0)
        {
            var start = ++row;
            row = GenerateSubtasksPart(ws, task, row) - 1;
            ws.Rows(start, row).Group();
            ws.Rows(start, row).Collapse();
            ws.Rows(start, row).Style.Font.SetFontSize(12);
        }

        return row + 1;
    }

    private static int GenerateSubtasksPart(IXLWorksheet ws, OnlyofficeTask task, int row)
    {
        ws.Rows(row, task.Subtasks.Count + row).Style
            .Alignment.SetIndent(4)
            .Alignment.SetWrapText(true)
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

        ws.Row(row).Style.Font.SetFontColor(XLColor.Gray);
        ws.Row(row).Height = 20;
        ws.Cell(row, 2).Value = "Статус";
        ws.Cell(row++, 1).Value = "Подзадача";

        foreach (var subtask in task.Subtasks.OrderByDescending(x => x.Status))
        {
            ws.Cell(row, 1).Value = subtask.Title;
            ws.Cell(row++, 2).Value = subtask.Status.GetDesc();
        }

        return row;
    }
}
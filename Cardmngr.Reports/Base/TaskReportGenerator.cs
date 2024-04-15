using System.Reflection;
using Cardmngr.Domain;
using Cardmngr.Domain.Entities;
using Cardmngr.Domain.Enums;
using Cardmngr.Reports.Base;
using Cardmngr.Reports.Extensions;
using ClosedXML.Excel;
using ClosedXML.Graphics;

using GroupedTask = System.Collections.Generic.IEnumerable<System.Linq.IGrouping<Cardmngr.Domain.ProjectInfo, System.Linq.IGrouping<Cardmngr.Domain.MilestoneInfo, Cardmngr.Domain.Entities.OnlyofficeTask>>>;

namespace Cardmngr.Reports;

public class TaskReportGenerator : IReportGenerator
{
    static TaskReportGenerator()
    {
        ConfigureFonts();
    }

    private static void ConfigureFonts()
    {
        var fallbackFontStream = Assembly
            .GetExecutingAssembly()
            .GetManifestResourceStream("mainFont.ttf");
        LoadOptions.DefaultGraphicEngine = DefaultGraphicEngine.CreateOnlyWithFonts(fallbackFontStream);
    }

    public IEnumerable<OnlyofficeTask>? Tasks { get; set; }
    public IEnumerable<OnlyofficeTaskStatus>? Statuses { get; set; }

    public byte[] GenerateReport()
    {
        using XLWorkbook wb = new();

        wb.PageOptions
            .SetPageOrientation(XLPageOrientation.Landscape);
        wb.Style
            .Font.SetFontSize(14)
            .Font.SetFontName("Times New Roman");

        var ws = wb.Worksheets.Add("Задачи проектов");

        ConfigureWorksheet(ws);
        GenerageHeader(ws);
        var row = 1;
        
        foreach (var groupedTask in GroupByProject(Tasks))
        {
            ws.Cell(row, 1).Style.Font.Bold = true;
            ws.Cell(row, 1).Value = groupedTask.Key.Title;

            row++;
            foreach (var milestone in groupedTask.OrderBy(x => x.Key != null))
            {
                GenerageMilestonePart(ws, milestone, ref row);
                row++;
            }
            row--;
        }

        ws.Columns().AdjustToContents();

        return wb.ToArray();
    }

    private static void ConfigureWorksheet(IXLWorksheet ws)
    {
        ws.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);        
        ws.PageSetup.SetPaperSize(XLPaperSize.A4Paper);
        ws.SetShowGridLines(false);
    }

    private static void GenerageHeader(IXLWorksheet ws)
    {
        ws.Cell("C1").Style.Font.Bold = true;
        ws.Cell("C1").Value = "Отчет создан:";
        ws.Cell("D1").Value = DateTime.Now.ToShortDateString();
    }

    private void GenerageMilestonePart(IXLWorksheet ws, IGrouping<MilestoneInfo, OnlyofficeTask> groupedTasks, ref int row)
    {
        if (groupedTasks.Key?.Title is { })
        {
            ws.Row(row).Style
                .Alignment.SetIndent(1)
                .Font.SetBold();

            ws.Cell(row, 1).Value = groupedTasks.Key.Title;
            row++;
        }

        GenerateTasksHeader(ws, row);
        row++;

        foreach (var task in groupedTasks.OrderBy(x => x.Deadline))
        {
            GenerateTaskPart(ws, task, row);
            row++;
        }
    }

    private static void GenerateTasksHeader(IXLWorksheet ws, int row)
    {
        ws.Row(row).Height = 30;

        ws.Row(row).Style            
            .Border.SetBottomBorder(XLBorderStyleValues.Thin)
            .Border.SetBottomBorderColor(XLColor.Gray)
            .Font.SetFontColor(XLColor.Gray);

        ws.Cell(row, 1).Style.Alignment.Indent = 2;
        ws.Cell(row, 1).Value = "Задача";
        ws.Cell(row, 2).Value = "Ответственные";
        ws.Cell(row, 3).Value = "Крайний срок";
        ws.Cell(row, 4).Value = "Статус";
    }

    private void GenerateTaskPart(IXLWorksheet ws, OnlyofficeTask task, int row)
    {
        ws.Row(row).Style
            .Border.SetBottomBorder(XLBorderStyleValues.Thin)
            .Border.SetBottomBorderColor(XLColor.Gray);

        ws.Cell(row, 1).Style.Alignment.Indent = 3;
        ws.Cell(row, 1).Value = task.Title;
        ws.Cell(row, 2).Value = string.Join(", ", task.Responsibles.Select(x => x.DisplayName));
        ws.Cell(row, 2).Style.Alignment.WrapText = true;
        ws.Cell(row, 3).Value = task.Deadline?.ToShortDateString() ?? "0";
        ws.Cell(row, 4).Value = GetStatusTitle(task);
    }

    private string GetStatusTitle(OnlyofficeTask task)
    {
        return task.TaskStatusId is null
            ? task.Status == Status.Open ? "Открыта" : "Закрыта"
            : Statuses?.Single(x => x.Id == task.TaskStatusId).Title ?? "Неизвестен";
    }

    private static GroupedTask GroupByProject(IEnumerable<OnlyofficeTask>? tasks)
    {
        return tasks?
            .GroupBy(x => x.ProjectOwner)
            .Select(projTasks => new ProjGrouping(projTasks.Key, projTasks))
            ?? [];
    }

    private class ProjGrouping(ProjectInfo key, IEnumerable<OnlyofficeTask> tasks) 
        : IGrouping<ProjectInfo, IGrouping<MilestoneInfo, OnlyofficeTask>>
    {
        public ProjectInfo Key => key;

        public IEnumerator<IGrouping<MilestoneInfo, OnlyofficeTask>> GetEnumerator()
        {
            return tasks
                .GroupBy(x => x.Milestone)
                .GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

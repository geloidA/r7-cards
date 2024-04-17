using System.Reflection;
using Cardmngr.Domain;
using Cardmngr.Domain.Entities;
using Cardmngr.Reports.Base;
using Cardmngr.Reports.Extensions;
using ClosedXML.Excel;
using ClosedXML.Graphics;

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

    public IEnumerable<OnlyofficeTask> Tasks { get; set; } = [];
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
        
        foreach (var groupedByProject in TaskReportData.Create(Tasks, x => new OnlyofficeTaskReportData(x, Statuses)).GroupedTasks)
        {
            ws.Row(row).Style.Fill.SetBackgroundColor(XLColor.FromHtml("#1294b7"));
            ws.Cell(row, 1).Style.Font.SetBold();
            
            ws.Cell(row, 1).Value = groupedByProject.Key.Title;

            row++;
            foreach (var milestone in groupedByProject.OrderBy(x => x.Key != null))
            {
                GenerageMilestonePart(ws, milestone, ref row);
                ws.Row(++row).Height = 15;
            }

            ws.Row(row).Delete();
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

    private static void GenerageMilestonePart(IXLWorksheet ws, IGrouping<MilestoneInfo, IOnlyofficeTaskReportData> groupedByMilestone, ref int row)
    {
        if (groupedByMilestone.Key?.Title is { })
        {
            ws.Row(row).Style
                .Fill.SetBackgroundColor(XLColor.FromHtml("#23b4b4"))
                .Alignment.SetIndent(1)
                .Font.SetBold();

            ws.Cell(row, 1).Value = groupedByMilestone.Key.Title;
            row++;
        }

        GenerateTasksHeader(ws, row);
        row++;

        foreach (var task in groupedByMilestone.OrderBy(x => x.Deadline))
        {
            GenerateTaskPart(ws, task, row);
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

        ws.Cell(row, 1).Style.Alignment.Indent = 2;
        ws.Cell(row, 1).Value = "Задача";
        ws.Cell(row, 2).Value = "Ответственные";
        ws.Cell(row, 3).Value = "Крайний срок";
        ws.Cell(row, 4).Value = "Статус";
    }

    private static void GenerateTaskPart(IXLWorksheet ws, IOnlyofficeTaskReportData task, int row)
    {
        ws.Row(row).Style
            .Border.SetBottomBorder(XLBorderStyleValues.Thin)
            .Border.SetBottomBorderColor(XLColor.Gray);

        ws.Cell(row, 1).Style.Alignment.Indent = 3;
        ws.Cell(row, 1).Value = task.Title;
        ws.Cell(row, 2).Value = task.Responsibles;
        ws.Cell(row, 2).Style.Alignment.WrapText = true;
        ws.Cell(row, 3).Value = task.Deadline;
        ws.Cell(row, 4).Value = task.StatusString;
    }
}
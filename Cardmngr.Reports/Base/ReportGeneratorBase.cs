using System.Reflection;
using ClosedXML.Excel;
using ClosedXML.Graphics;

namespace Cardmngr.Reports.Base;

public abstract class ReportGeneratorBase : IReportGenerator
{
    static ReportGeneratorBase()
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

    protected static void ConfigureWorkbook(IXLWorkbook wb)
    {
        wb.PageOptions
            .SetPageOrientation(XLPageOrientation.Landscape);
        wb.Style
            .Font.SetFontSize(14)
            .Font.SetFontName("Times New Roman");
    }

    protected static void ConfigureWorksheet(IXLWorksheet ws)
    {
        ws.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        ws.PageSetup.SetPaperSize(XLPaperSize.A4Paper);
        ws.SetShowGridLines(false);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ws"></param>
    /// <param name="title"></param>
    /// <param name="lastCellRow">Last cell row</param>
    /// <param name="lastCellColumn">Last cell column</param>
    /// <returns>Begin of empty row</returns>
    protected static int GenerateHeader(IXLWorksheet ws, string title,
        int lastCellRow = 1,
        int lastCellColumn = 2)
    {
        ws.Row(1).Height = 60;
        ws.Range(1, 1, lastCellRow, lastCellColumn).Merge();
        ws.Cell("A1").Style
            .Font.SetBold()
            .Font.SetFontSize(24)
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        ws.Cell("A1").Value = title;

        ws.Cell(1, lastCellColumn).Style.Font.Bold = true;
        ws.Cell(1, lastCellColumn + 1).Value = "Отчет создан:";
        ws.Cell(1, lastCellColumn + 2).Value = DateTime.Now.ToShortDateString();

        return 2;
    }

    public abstract byte[] GenerateReport();
}

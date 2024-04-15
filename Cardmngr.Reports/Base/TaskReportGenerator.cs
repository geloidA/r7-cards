using System.Reflection;
using Cardmngr.Domain.Entities;
using Cardmngr.Reports.Base;
using ClosedXML.Excel;
using ClosedXML.Graphics;

namespace Cardmngr.Reports;

public class TaskReportGenerator : IReportGenerator
{
    public TaskReportGenerator()
    {
        var fallbackFontStream = Assembly
            .GetExecutingAssembly()
            .GetManifestResourceStream("LiberationSerif-Regular.ttf");

        LoadOptions.DefaultGraphicEngine = DefaultGraphicEngine.CreateOnlyWithFonts(fallbackFontStream);
    }

    public List<OnlyofficeTask>? Tasks { get; set; }

    public byte[] GenerateReport()
    {
        using XLWorkbook wb = new();
        var ws = wb.Worksheets.Add("testSheet");
        ws.Cell("B3").Value = "test";
        ws.Cell("B8").Value = "test";
        ws.Columns().AdjustToContents();

        using MemoryStream stream = new();
        wb.SaveAs(stream);
        stream.Position = 0;
        return stream.ToArray();
    }
}

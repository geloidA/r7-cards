using Cardmngr.Domain.Entities;
using Cardmngr.Reports.Extensions;
using ClosedXML.Excel;

namespace Cardmngr.Reports;

public class CreatedTaskReportGenerator : ReportGeneratorBase
{
    public IEnumerable<OnlyofficeTask> Tasks { get; set; } = [];

    public override byte[] GenerateReport()
    {
        using XLWorkbook wb = new();

        ConfigureWorkbook(wb);

        var ws = wb.Worksheets.Add("Созданные задачи");

        ConfigureWorksheet(ws);

        var row = GenerateHeader(ws, "Созданные задачи");

        foreach (var groupByUser in Tasks.GroupBy(x => x.CreatedBy).OrderBy(x => x.Key.DisplayName))
        {
            row = GenerateUserHeader(ws, row, groupByUser);
        }

        return wb.ToArray();
    }

    private static int GenerateUserHeader(IXLWorksheet ws, int row, IGrouping<UserInfo, OnlyofficeTask> groupByUser)
    {
        ws.Row(row).Style.Fill.SetBackgroundColor(XLColor.LightGray);
        ws.Cell(row, 1).Style.Font.SetBold();

        ws.Cell(row, 1).Value = $"{groupByUser.Key.DisplayName} ({groupByUser.Count()})";
        return row + 1;
    }
}

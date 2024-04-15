using ClosedXML.Excel;

namespace Cardmngr.Reports.Extensions;

public static class XLWorkbookExtensions
{
    public static byte[] ToArray(this IXLWorkbook wb)
    {
        using MemoryStream stream = new();
        wb.SaveAs(stream);
        stream.Position = 0;
        return stream.ToArray();
    }
}

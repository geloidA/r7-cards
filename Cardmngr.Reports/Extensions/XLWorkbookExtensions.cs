using ClosedXML.Excel;

namespace Cardmngr.Reports.Extensions;

public static class XLWorkbookExtensions
{
    /// <summary>
    /// Преобразует <see cref="IXLWorkbook"/> в байтовый массив.
    /// </summary>
    /// <param name="wb">Экземпляр <see cref="IXLWorkbook"/>.</param>
    /// <returns>Байтовый массив, содержащий файл Excel.</returns>
    public static byte[] ToArray(this IXLWorkbook wb)
    {
        using MemoryStream stream = new();
        wb.SaveAs(stream);
        stream.Position = 0;
        return stream.ToArray();
    }
}

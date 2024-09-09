using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Cardmngr.Report;

public class ReportJsModule(IJSRuntime jsRuntime) : JSModule(jsRuntime, "/js/saveFile.js")
{
    public ValueTask SaveAsAsync(string fileName, byte[] fileData)
    {
        return InvokeVoidAsync("saveAs", Convert.ToBase64String(fileData), fileName);
    }
}

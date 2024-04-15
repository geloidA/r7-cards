using Cardmngr.Utils;
using Microsoft.JSInterop;

namespace Cardmngr.Report;

public class ReportJSModule(IJSRuntime jsRuntime) : IAsyncDisposable
{
    private readonly JSModule module = jsRuntime.LoadJSModule("/js/GeneratedJS/saveFile.js");

    public ValueTask SaveAsAsync(string fileName, byte[] fileData)
    {
        return module.InvokeVoidAsync("saveAs", Convert.ToBase64String(fileData), fileName);
    }

    public ValueTask DisposeAsync()
    {
        return module.DisposeAsync();
    }
}

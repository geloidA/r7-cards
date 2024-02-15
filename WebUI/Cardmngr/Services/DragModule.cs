using Cardmngr.Utils;
using Microsoft.JSInterop;

namespace Cardmngr;

public sealed class DragModule(IJSRuntime jSRuntime) : IAsyncDisposable
{
    private readonly JSModule module = jSRuntime.LoadJSModule("./js/GeneratedJS/DragModule.js");

    public ValueTask StartDrag<T>(DotNetObjectReference<T> callbackObject, string methodName)
        where T : class
    {
        return module.InvokeVoidAsync("startDrag", callbackObject, methodName);
    }

    public ValueTask DisposeAsync()
    {
        return module.DisposeAsync();
    }
}

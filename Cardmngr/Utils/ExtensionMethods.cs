using BlazorCards;
using Microsoft.JSInterop;

namespace Cardmngr.Utils;

public static class ExtensionMethods
{
    /// <summary>
    /// Begins asynchronously loading a JS module from the given path.
    /// </summary>
    /// <remarks>
    /// The module can be used immediately, and calls will automatically
    /// begin executing once the module is loaded.
    /// </remarks>
    public static JSModule LoadJSModule(this IJSRuntime jsRuntime, string path)
    {
        return new JSModule(jsRuntime.InvokeAsync<IJSObjectReference>("import", path).AsTask());
    }
}

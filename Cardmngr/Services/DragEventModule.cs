using Cardmngr.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Cardmngr;

public sealed class DragEventModule(IJSRuntime jSRuntime) : IAsyncDisposable
{    
    private readonly JSModule module = jSRuntime.LoadJSModule("./js/GeneratedJS/DragEventModule.js");
    private IJSObjectReference dragModuleRef = null!;

    public async ValueTask InitializeModule<T>(ElementReference elementReference, DotNetObjectReference<T> dotNetObjectReference)
        where T : class
    {
        dragModuleRef = await module.InvokeAsync<IJSObjectReference>("initializeModule", elementReference, dotNetObjectReference);
    }

    public ValueTask OnDragEnter(string methodName, bool stopPropagation = false, bool preventDefault = false)
    {
        return dragModuleRef.InvokeVoidAsync("onDragEnter", methodName, stopPropagation, preventDefault);
    }

    public ValueTask OnDragOver(string methodName, int delay = 0, bool stopPropagation = false, bool preventDefault = false)
    {
        return dragModuleRef.InvokeVoidAsync("onDragOver", delay, methodName, stopPropagation, preventDefault);
    }

    public ValueTask OnDragLeave(string methodName, bool stopPropagation = false, bool preventDefault = false)
    {
        return dragModuleRef.InvokeVoidAsync("onDragLeave", methodName, stopPropagation, preventDefault);
    }

    public async ValueTask DisposeAsync()
    {
        await dragModuleRef.InvokeVoidAsync("dispose");
        await dragModuleRef.DisposeAsync();
    }
}

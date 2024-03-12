using Cardmngr.Utils;
using Microsoft.JSInterop;

namespace Cardmngr.Notification;

public class NotificationJSModule(IJSRuntime jsRuntime) : IAsyncDisposable
{
    private readonly JSModule module = jsRuntime.LoadJSModule("/js/GeneratedJS/notifications.js");

    public PermissionType PermissionStatus { get; private set; }

    public ValueTask CreateAsync(string title, NotificationOptions? options = null)
    {
        return module.InvokeVoidAsync("create", title, options);
    }

    public async ValueTask<PermissionType> RequestPermissionAsync()
    {
        var permission = await module.InvokeAsync<string>("requestPermission");

        if (permission.Equals("granted", StringComparison.InvariantCultureIgnoreCase))
            PermissionStatus = PermissionType.Granted;

        if (permission.Equals("denied", StringComparison.InvariantCultureIgnoreCase))
            PermissionStatus = PermissionType.Denied;

        return PermissionStatus;
    }

    public ValueTask<bool> IsSupportedByBrowserAsync()
    {
        return module.InvokeAsync<bool>("isSupported");
    }

    public ValueTask DisposeAsync()
    {
        return module.DisposeAsync();
    }
}
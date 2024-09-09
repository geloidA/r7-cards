using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Cardmngr.Notification;

public class NotificationJsModule(IJSRuntime jsRuntime) : JSModule(jsRuntime, "/js/notifications.js")
{
    private PermissionType PermissionStatus { get; set; }

    public ValueTask CreateAsync(string title, NotificationOptions? options = null)
    {
        return options is null 
            ? InvokeVoidAsync("create", title)
            : InvokeVoidAsync("create", title, options);
    }

    public async ValueTask<PermissionType> RequestPermissionAsync()
    {
        var permission = await InvokeAsync<string>("requestPermission").ConfigureAwait(false);

        if (permission.Equals("granted", StringComparison.InvariantCultureIgnoreCase))
            PermissionStatus = PermissionType.Granted;

        if (permission.Equals("denied", StringComparison.InvariantCultureIgnoreCase))
            PermissionStatus = PermissionType.Denied;

        return PermissionStatus;
    }

    public ValueTask<bool> IsSupportedByBrowserAsync()
    {
        return InvokeAsync<bool>("isSupported");
    }
}
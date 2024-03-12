using Cardmngr.Notification;

namespace Cardmngr.Services;

public sealed class NotificationService(NotificationJSModule notification) : IAsyncDisposable
{
    private readonly NotificationJSModule notification = notification;

    public async ValueTask<PermissionType> RequestPermissionAsync()
    {
        var isSupportedByBrowser = await notification.IsSupportedByBrowserAsync();

        if (isSupportedByBrowser)
        {
            return await notification.RequestPermissionAsync();
        }
        
        return default;
    }

    public ValueTask CreateAsync(string title, NotificationOptions? options = null)
    {
        return notification.CreateAsync(title, options); 
    }

    public ValueTask<bool> IsSupportedByBrowserAsync() => notification.IsSupportedByBrowserAsync();

    public async ValueTask DisposeAsync() => await notification.DisposeAsync();
}

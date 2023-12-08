using System.Globalization;
using Microsoft.JSInterop;

namespace Cardmngr.Services;

public static class ZoomHandler
{
    public static double Zoom { get; set; } = 1d;

    public static string ZoomCSS => Zoom.ToString(CultureInfo.InvariantCulture);

    public static void ZoomBy(double factor)
    {
        Zoom *= factor;
    }

    [JSInvokable]
    public static void SetZoom(double zoom)
    {
        Zoom = zoom;
    }
}

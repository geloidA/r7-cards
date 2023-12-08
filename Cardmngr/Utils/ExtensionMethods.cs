using BlazorCards;

namespace Cardmngr.Utils;

public static class ExtensionMethods
{
    public static Vector2 GetClientPos(this Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
    {
        return new Vector2(e.ClientX, e.ClientY);
    }
}

using Microsoft.AspNetCore.Components;

namespace Cardmngr.Shared.Extensions;

public static class StringExtensions
{
    public static MarkupString RenderHtml(this string htmlString)
    {
        return new MarkupString(htmlString);
    }
}

using System.Drawing;
using System.Globalization;

namespace Cardmngr.Extensions;

public static class ColorExtensions
{
    public static string ToRgbaText(this Color color) 
        => $"rgba({color.R}, {color.G}, {color.B}, {(color.A / 255.0).ToString(CultureInfo.InvariantCulture)})";

    public static Color WithAlpha(this Color color, double alpha)
    {
        if (alpha > 1 || alpha < 0) throw new ArgumentOutOfRangeException(nameof(alpha));
        
        return Color.FromArgb((int)(color.A * alpha), color.R, color.G, color.B);
    }
}

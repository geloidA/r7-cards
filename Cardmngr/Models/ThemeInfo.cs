namespace Cardmngr.Models;

public class ThemeInfo
{
    public string? ColorTheme { get; set; }

    public static ThemeInfo Default => new() { ColorTheme = "light" };
}

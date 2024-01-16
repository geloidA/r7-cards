namespace BlazorCards.Core;

public interface IUIElement
{
    string? CssName { get; set; }
    string? CssColor { get; set; }
    object? Data { get; set; }
}

public abstract class UIElementBase : IUIElement
{
    public string? CssName { get; set; }
    public string? CssColor { get; set; }
    public object? Data { get; set; }
}

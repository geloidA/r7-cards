namespace BlazorCards.Core;

public abstract class UIElementBase : IUIElement
{
    public string? CssName { get; set; }
    public string? CssColor { get; set; }
    public object? Data { get; set; }
}

namespace BlazorCards.Core;

public interface IUIElement
{
    string? CssName { get; set; }
    string? CssColor { get; set; }
    object? Data { get; set; }
}

public interface IWorkspaceElement : IUIElement
{
    Vector2 Pos { get; set; }    
    event Action PosChanged;

    event Action LayoutChanged;
    void OnLayoutChanged();
}

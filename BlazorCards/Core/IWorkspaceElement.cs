namespace BlazorCards.Core;

public interface IUIElement
{
    string CssName { get; }
    string CssColor { get; }
}

public interface IWorkspaceElement : IUIElement
{
    Vector2 Pos { get; set; }    
    event Action PosChanged;
}

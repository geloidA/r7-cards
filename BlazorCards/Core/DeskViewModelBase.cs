namespace BlazorCards.Core;

public interface IDesk
{
    Vector2 Pos { get; }
    string Title { get; }
    void AddItem(IDeskItem deskItem);
}

public interface IDeskViewModel : IDesk
{
    
}

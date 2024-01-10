using BlazorCards.Core;

namespace BlazorCards;

public class Card(string title) : UIElementBase, ICardDao, IDisposable
{
    internal Card(string title, BoardColumn column) : this(title)
    {
        Column = column;
    }

    public string? Title { get; set; } = title;
    public string? Description { get; set; }
    public BoardColumn? Column { get; internal set; }

    public event Action<Card>? DragStart;

    public void Dispose()
    {
        DragStart = null;
    }

    public void OnDragStart() => DragStart?.Invoke(this);
}

public interface ICardDao
{
    string? Title { get; set; }
    string? Description { get; set; }
}
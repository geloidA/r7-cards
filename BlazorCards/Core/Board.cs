using BlazorCards.Utils;

namespace BlazorCards.Core;

public class Board(string title) : ObservableLinkedCollection<BoardColumn>, IWorkspaceElement
{
    public Board(IEnumerable<IBoardColumnDao> columns, IEnumerable<IEnumerable<ICardDao>> cards, string title, object? data = null) : this(title)
    {
        if (columns.Count() != cards.Count()) throw new ArgumentException("Columns and cards count must be equal");

        items = new(columns
            .Zip(cards)
            .Select(x => new BoardColumn(x.Second, x.First.Title!, this) { Data = x.First }));
        
        Data = data;
    }

    private Vector2 pos;
    public string Title { get; set; } = title;
    public Vector2 Pos
    {
        get => pos;
        set
        {
            pos = value;
            PosChanged?.Invoke();
        }
    }

    public override void Add(BoardColumn item)
    {
        base.Add(item);
        item.Board = this;
    }

    public override void AddAfter(BoardColumn target, BoardColumn item)
    {
        base.AddAfter(target, item);
        item.Board = this;
    }

    public override void AddBefore(BoardColumn target, BoardColumn item)
    {
        base.AddBefore(target, item);
        item.Board = this;
    }

    public override void Remove(BoardColumn item)
    {
        base.Remove(item);
        item.Board = null;
    }

    public void OnLayoutChanged() => LayoutChanged?.Invoke();

    public string? CssName { get; set; }

    public string? CssColor { get; set; }
    public object? Data { get; set; }

    public event Action? PosChanged;
    public event Action? LayoutChanged;
}

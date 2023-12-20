using BlazorCards.Utils;

namespace BlazorCards.Core;

public class BoardColumn : ObservableLinkedCollection<Card>, IUIElement, IBoardColumnDao
{
    public BoardColumn(string title)
    {
        Title = title;
    }
    
    public BoardColumn(string title, Board board) : this(Enumerable.Empty<ICardDao>(), title, board)
    {
    }

    public BoardColumn(IEnumerable<ICardDao> cards, string title, Board board)
    {
        items = new(cards.Select(x => new Card(x.Title, this) { Description = x.Description }));
        Title = title;
        Board = board;
    }

    public string Title { get; set; }
    public Board? Board { get; internal set; }
    public string? CssName { get; set; }
    public string? CssColor { get; set; }

    public override void Add(Card item)
    {
        base.Add(item);
        item.Column = this;
    }

    public override void AddAfter(Card target, Card item)
    {
        base.AddAfter(target, item);
        item.Column = this;
    }

    public override void AddBefore(Card target, Card item)
    {
        base.AddBefore(target, item);
        item.Column = this;
    }

    public override void Remove(Card item)
    {
        base.Remove(item);
        item.Column = null;
    }
}

public interface IBoardColumnDao
{
    string Title { get; set; }
}

namespace BlazorCards.Core;

public interface IBoardColumn : IUIElement, IContainer<ICardTrack>
{
    string? Title { get; set; }
}

public abstract class BoardColumnViewModelBase : IBoardColumn
{
    public string? Title { get; set; }

    public abstract IEnumerable<ICardTrack> Items { get; }

    public abstract int Count { get; }

    public abstract string CssName { get; }

    public abstract string CssColor { get; }

    public abstract void Add(ICardTrack item);

    public abstract void Remove(ICardTrack item);
}

using BlazorCards.Core;
using static BlazorCards.Core.CardBase;

namespace Cardmngr;

public class Board : BoardViewModelBase
{
    private readonly List<IBoardColumn> columns = [];

    public Board(string title) { Title = title; }

    public Board(string title, IEnumerable<IBoardColumn> items) : this(title) { columns = items.ToList(); }

    public override string CssName => "board";

    public override string CssColor => "";

    public override IEnumerable<IBoardColumn> Items => columns;

    public override int Count => columns.Sum(x => x.Count);

    public override IEnumerable<CardBase> TotalCards => columns.SelectMany(c => c.Items.SelectMany(t => t));

    public override void Add(IBoardColumn item)
    {
        columns.Add(item);
    }

    public override void Remove(IBoardColumn item)
    {
        columns.Remove(item);
    }
}

public class BoardColumn : BoardColumnViewModelBase
{
    private readonly List<ICardTrack> tracks = [];

    public BoardColumn(string title) { Title = title; }

    public BoardColumn(string title, IEnumerable<ICardTrack> items) : this(title) { tracks = items.ToList(); }

    public override IEnumerable<ICardTrack> Items => tracks;

    public override int Count => tracks.Sum(x => x.Count);

    public override string CssName => "board-column";

    public override string CssColor => "";

    public override void Add(ICardTrack item)
    {
        tracks.Add(item);
    }

    public override void Remove(ICardTrack item)
    {
        tracks.Remove(item);
    }
}

public class CardTrack : CardTrackViewModelBase
{
    public CardTrack(string title, IEnumerable<CardBase> items) : base(items)
    {
        Title = title;
    }

    public override string CssName => "card-track";

    public override string CssColor => "";
}

public class Card : CardBase
{
    public override string CssName => "my-card";

    public override string CssColor => "";

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Card other) return false;
        return ReferenceEquals(this, other);
    }
}
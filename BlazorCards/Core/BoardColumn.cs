﻿using BlazorCards.Utils;

namespace BlazorCards.Core;

public class BoardColumn(string title) : ObservableLinkedCollection<Card>, IUIElement, IBoardColumnDao
{
    public BoardColumn(IEnumerable<ICardDao> cards, string title) : this(title)
    {
        items = new(cards.Select(x => new Card(x.Title!, this) { Description = x.Description }));
    }
    
    internal BoardColumn(string title, Board board) : this([], title, board)
    {
    }

    internal BoardColumn(IEnumerable<ICardDao> cards, string title, Board board) : this(title)
    {
        items = new(cards.Select(x => new Card(x.Title!, this) 
        { 
            Description = x.Description,
            Data = x
        }));
        Board = board;
    }

    public string? Title { get; set; } = title;
    public Board? Board { get; internal set; }
    public string? CssName { get; set; }
    public string? CssColor { get; set; }
    public object? Data { get; set; }

    public event Action? LayoutChanged;

    public void OnLayoutChanged() => LayoutChanged?.Invoke();

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
    string? Title { get; set; }
}
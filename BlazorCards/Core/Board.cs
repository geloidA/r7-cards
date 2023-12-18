namespace BlazorCards.Core;

public interface IBoard : IWorkspaceElement, IContainer<IBoardColumn>
{
    string? Title { get; set; }
}

public interface IBoardViewModel : IBoard
{
    bool IsCollapsed { get; set; }

    IEnumerable<CardBase> TotalCards { get; }

    event Action LayoutChanged;
    void OnLayoutChanged();
}

public abstract class BoardViewModelBase : IBoardViewModel
{
    public bool IsCollapsed { get; set; }
    public string? Title { get; set; }

    private Vector2 pos;
    public Vector2 Pos
    { 
        get => pos; 
        set
        {
            pos = value;
            PosChanged?.Invoke();
        } 
    }

    public abstract string CssName { get; }

    public abstract string CssColor { get; }

    public abstract IEnumerable<IBoardColumn> Items { get; }

    public abstract int Count { get; }

    public abstract IEnumerable<CardBase> TotalCards { get; }

    public event Action? LayoutChanged;
    public event Action? PosChanged;

    public void OnLayoutChanged() => LayoutChanged?.Invoke();

    public abstract void Add(IBoardColumn item);

    public abstract void Remove(IBoardColumn item);
}

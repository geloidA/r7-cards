namespace BlazorCards.Core;

public interface IWorkspace : IContainer<IWorkspaceElement>
{
    string? Title { get; set; }
}

public abstract class WorkspaceViewModelBase : IWorkspace
{
    public string? Title { get; set; }

    public abstract IEnumerable<IWorkspaceElement> Items { get; }

    public abstract int Count { get; }

    public abstract void Add(IWorkspaceElement item);

    public abstract void Remove(IWorkspaceElement item);
}

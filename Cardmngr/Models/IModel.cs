namespace Cardmngr.Models;

public interface IModel
{
    event Action ModelChanged;
}

public abstract class ModelBase : IModel
{
    public event Action? ModelChanged;

    protected void OnModelChanged() => ModelChanged?.Invoke();
}

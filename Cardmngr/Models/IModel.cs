using Onlyoffice.Api.Models;

namespace Cardmngr.Models;

public interface IModel
{
    event Action ModelChanged;
}

public abstract class ModelBase : IModel
{
    public DateTime Created { get; protected set; }
    public IUser? CreatedBy { get; protected set; }
    public DateTime Updated { get; protected set;}

    public event Action? ModelChanged;

    protected void OnModelChanged() => ModelChanged?.Invoke();
}

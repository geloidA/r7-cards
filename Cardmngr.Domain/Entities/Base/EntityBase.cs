namespace Cardmngr.Domain.Entities.Base;

public abstract record class EntityBase<TId>
{
    public TId Id { get; init; }
}

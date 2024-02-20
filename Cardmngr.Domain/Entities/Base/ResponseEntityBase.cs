namespace Cardmngr.Domain.Entities.Base;

public abstract record ResponseEntityBase<TId> : EntityBase<TId>
{
    public bool CanEdit { get; init; }
}

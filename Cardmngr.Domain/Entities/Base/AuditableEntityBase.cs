namespace Cardmngr.Domain.Entities.Base
{
    public abstract record class AuditableEntityBase<TId> : EntityBase<TId>
    {
        public DateTime Created { get; init; }
        public DateTime Updated { get; init; }
        public UserInfo CreatedBy { get; init; }
    }
}

namespace Cardmngr.Domain.Entities.Base
{
    public abstract class AuditableEntityBase<TId> : EntityBase<TId>
    {
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public UserInfo CreatedBy { get; set; }
    }
}

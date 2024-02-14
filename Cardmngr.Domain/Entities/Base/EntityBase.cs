namespace Cardmngr.Domain.Entities.Base
{
    public abstract class EntityBase<TId>
    {
        public TId Id { get; set; }
    }
}

using Cardmngr.Domain.Entities.Base;

namespace Cardmngr.Domain.Entities;

public record class Comment : AuditableEntityBase<string>
{
    public string Text { get; init; }
    public string ParentId { get; init; }
    public bool Inactive { get; init; }
    public bool CanEdit { get; init; }
}

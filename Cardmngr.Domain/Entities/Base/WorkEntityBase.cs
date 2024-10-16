namespace Cardmngr.Domain.Entities.Base;

public abstract record class WorkEntityBase : AuditableEntityBase<int>, ITitled
{
    public string Title { get; init; }
    public string Description { get; init; }
    public bool CanEdit { get; init; } 
    public bool CanDelete { get; init; }
    public abstract IEnumerable<UserInfo> GetResponsibles();
}

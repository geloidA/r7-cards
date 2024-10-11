using Cardmngr.Domain.Entities.Base;
using Cardmngr.Domain.Enums;

namespace Cardmngr.Domain.Entities;

public sealed record Project : WorkEntityBase
{
    public ProjectStatus Status { get; init; }
    public UserInfo Responsible { get; init; }
    public bool IsPrivate { get; init; }
    public bool IsFollow { get; set; }

    public bool Equals(Project other)
    {
        if (other is null) return false;

        return Id == other.Id &&
            Title == other.Title &&
            Description == other.Description &&
            Status == other.Status &&
            Responsible.Id == other.Responsible.Id &&
            IsPrivate == other.IsPrivate;
    }

    public override int GetHashCode() => Id;

    public override IEnumerable<UserInfo> GetResponsibles()
    {
        return [Responsible];
    }
}

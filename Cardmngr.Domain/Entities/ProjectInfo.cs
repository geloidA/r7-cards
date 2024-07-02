using Cardmngr.Domain.Entities.Base;
using Cardmngr.Domain.Enums;

namespace Cardmngr.Domain.Entities;

public record ProjectInfo : EntityBase<int>
{
    public string Title { get; init; }
    public ProjectStatus Status { get; init; }
    public bool IsPrivate { get; init; }
}

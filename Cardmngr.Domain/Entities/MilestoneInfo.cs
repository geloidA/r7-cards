using Cardmngr.Domain.Entities.Base;

namespace Cardmngr.Domain.Entities;

public record MilestoneInfo(string Title, DateTime Deadline) : EntityBase<int>, ITitled { }

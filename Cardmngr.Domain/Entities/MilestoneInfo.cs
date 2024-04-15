using Cardmngr.Domain.Entities.Base;

namespace Cardmngr.Domain;

public record class MilestoneInfo(string Title, DateTime Deadline) : EntityBase<int> { }

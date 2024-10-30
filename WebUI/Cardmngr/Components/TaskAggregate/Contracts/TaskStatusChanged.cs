using Cardmngr.Domain.Entities;

namespace Cardmngr.Components.TaskAggregate.Contracts;

public class TaskStatusChanged
{
    public required OnlyofficeTask Task { get; init; }
}

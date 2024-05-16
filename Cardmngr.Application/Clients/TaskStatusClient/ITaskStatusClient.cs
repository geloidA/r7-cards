using Cardmngr.Domain.Entities;

namespace Cardmngr.Application;

public interface ITaskStatusClient
{
    IAsyncEnumerable<OnlyofficeTaskStatus> GetAllAsync();
}

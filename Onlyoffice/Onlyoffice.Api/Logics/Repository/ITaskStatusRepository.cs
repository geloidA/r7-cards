using Onlyoffice.Api.Models;

namespace Onlyoffice.Api.Logics.Repository;

public interface ITaskStatusRepository
{
    IAsyncEnumerable<TaskStatusDto> GetAllAsync();
}

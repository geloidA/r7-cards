using AutoMapper;
using Cardmngr.Domain.Entities;
using Onlyoffice.Api.Logics.Repository;

namespace Cardmngr.Application;

public class TaskStatusClient(ITaskStatusRepository taskStatusRepository, IMapper mapper) : ITaskStatusClient
{
    public IAsyncEnumerable<OnlyofficeTaskStatus> GetAllAsync()
    {
        return taskStatusRepository
            .GetAllAsync()
            .Select(mapper.Map<OnlyofficeTaskStatus>);
    }
}

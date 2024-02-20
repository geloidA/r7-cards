using Cardmngr.Application.Clients.Base;
using Cardmngr.Domain.Entities;
using Onlyoffice.Api.Models;

namespace Cardmngr.Application.Clients.Task;

public interface ITaskClient : IEntityClient<OnlyofficeTask, TaskUpdateData>
{
    Task<OnlyofficeTask> UpdateTaskStatusAsync(int taskId, OnlyofficeTaskStatus status);
}

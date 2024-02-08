using Onlyoffice.Api.Models;

using MyTask = Onlyoffice.Api.Models.Task;
using MyTaskStatus = Onlyoffice.Api.Models.TaskStatus;

namespace Cardmngr.Server.Services;

public interface IProjectFileService
{
    Task<Project> GetProject();
    IAsyncEnumerable<MyTaskStatus> GetTaskStatuses();
    IAsyncEnumerable<MyTask> GetTasksAsync();
}

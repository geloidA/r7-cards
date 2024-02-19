using Onlyoffice.Api.Models;

using Task = System.Threading.Tasks.Task;
using OnlyofficeTask = Onlyoffice.Api.Models.TaskDto;
using OnlyofficeTaskStatus = Onlyoffice.Api.Models.TaskStatusDto;

namespace Cardmngr.Server.Services;

public interface IProjectFileService
{
    Task<ProjectDto> GetProject();
    IAsyncEnumerable<OnlyofficeTaskStatus> GetTaskStatuses();
    IAsyncEnumerable<OnlyofficeTask> GetTasksAsync(string guid);
    Task UpdateTaskStatus(int taskId, int statusId);
    Task<OnlyofficeTask> DeleteTaskAsync(int taskId);
    Task<OnlyofficeTask> UpdateTask(int taskId, TaskUpdateData state);
    Task<OnlyofficeTask> CreateTask(string guid, TaskUpdateData state);
}

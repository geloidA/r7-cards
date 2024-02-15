using Onlyoffice.Api.Models;

using Task = System.Threading.Tasks.Task;
using OnlyofficeTask = Onlyoffice.Api.Models.Task;
using OnlyofficeTaskStatus = Onlyoffice.Api.Models.TaskStatus;

namespace Cardmngr.Server.Services;

public interface IProjectFileService
{
    Task<Project> GetProject();
    IAsyncEnumerable<OnlyofficeTaskStatus> GetTaskStatuses();
    IAsyncEnumerable<OnlyofficeTask> GetTasksAsync(string guid);
    Task UpdateTaskStatus(int taskId, int statusId);
    Task<OnlyofficeTask> DeleteTaskAsync(int taskId);
    Task<OnlyofficeTask> UpdateTask(int taskId, UpdatedStateTask state);
    Task<OnlyofficeTask> CreateTask(string guid, UpdatedStateTask state);
}

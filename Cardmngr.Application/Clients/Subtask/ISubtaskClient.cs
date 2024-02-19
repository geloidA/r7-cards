using Onlyoffice.Api.Models;

namespace Cardmngr.Application.Clients.Subtask
{
    public interface ISubtaskClient
    {
        Task<Domain.Entities.Subtask> CreateAsync(int taskId, SubtaskUpdateData updateData);
        Task<Domain.Entities.Subtask> UpdateAsync(int taskId, int subtaskId, SubtaskUpdateData updateData);
        Task<Domain.Entities.Subtask> RemoveAsync(int taskId, int subtaskId);
    }
}

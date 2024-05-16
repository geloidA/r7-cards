using AutoMapper;
using Cardmngr.Domain.Enums;
using Onlyoffice.Api.Logics.Repository;
using Onlyoffice.Api.Models;

namespace Cardmngr.Application.Clients.Subtask
{
    public class SubtaskClient(ISubtaskRepository subtaskRepository, IMapper mapper) : ISubtaskClient
    {
        public async Task<Domain.Entities.Subtask> CreateAsync(int taskId, SubtaskUpdateData updateData)
        {
            if (updateData.Title == null) throw new NullReferenceException("Sutask's title can't be null");
            var subtaskDto = await subtaskRepository.CreateAsync(taskId, updateData.Title, updateData.Responsible);
            return mapper.Map<Domain.Entities.Subtask>(subtaskDto);
        }

        public async Task<Domain.Entities.Subtask> RemoveAsync(int taskId, int subtaskId)
        {
            return mapper.Map<Domain.Entities.Subtask>(await subtaskRepository.DeleteAsync(taskId, subtaskId));
        }

        public async Task<Domain.Entities.Subtask> UpdateAsync(int taskId, int subtaskId, SubtaskUpdateData updateData)
        {
            return mapper.Map<Domain.Entities.Subtask>(await subtaskRepository.UpdateAsync(taskId, subtaskId, updateData));
        }

        public async Task<Domain.Entities.Subtask> UpdateSubtaskStatusAsync(int taskId, int subtaskId, Status status)
        {
            var subtaskDto = await subtaskRepository.UpdateStatusAsync(taskId, subtaskId, mapper.Map<Onlyoffice.Api.Common.Status>(status));
            return mapper.Map<Domain.Entities.Subtask>(subtaskDto);
        }
    }
}

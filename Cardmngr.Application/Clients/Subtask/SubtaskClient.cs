using System.Security.AccessControl;
using AutoMapper;
using Cardmngr.Domain.Enums;
using Onlyoffice.Api.Logics;
using Onlyoffice.Api.Models;

namespace Cardmngr.Application.Clients.Subtask
{
    public class SubtaskClient(IProjectApi projectApi, IMapper mapper) : ISubtaskClient
    {
        private readonly IProjectApi projectApi = projectApi;
        private readonly IMapper mapper = mapper;

        public async Task<Domain.Entities.Subtask> CreateAsync(int taskId, SubtaskUpdateData updateData)
        {
            if (updateData.Title == null) throw new NullReferenceException("Sutask's title can't be null");
            var subtaskDto = await projectApi.CreateSubtaskAsync(taskId, updateData.Title, updateData.Responsible);
            return mapper.Map<Domain.Entities.Subtask>(subtaskDto);
        }

        public async Task<Domain.Entities.Subtask> RemoveAsync(int taskId, int subtaskId)
        {
            var subtaskDto = await projectApi.DeleteSubtaskAsync(taskId, subtaskId);
            return mapper.Map<Domain.Entities.Subtask>(subtaskDto);
        }

        public async Task<Domain.Entities.Subtask> UpdateAsync(int taskId, int subtaskId, SubtaskUpdateData updateData)
        {
            var subtaskDto = await projectApi.UpdateSubtaskAsync(taskId, subtaskId, updateData);
            return mapper.Map<Domain.Entities.Subtask>(subtaskDto);
        }

        public async Task<Domain.Entities.Subtask> UpdateSubtaskStatusAsync(int taskId, int subtaskId, Status status)
        {
            var subtaskDto = await projectApi.UpdateSubtaskStatusAsync(taskId, subtaskId, mapper.Map<Onlyoffice.Api.Common.Status>(status));
            return mapper.Map<Domain.Entities.Subtask>(subtaskDto);
        }
    }
}

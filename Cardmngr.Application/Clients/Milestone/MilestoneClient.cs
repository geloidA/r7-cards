using AutoMapper;
using Onlyoffice.Api.Logics;
using Onlyoffice.Api.Models;

namespace Cardmngr.Application.Clients.Milestone
{
    public class MilestoneClient(IProjectApi projectApi, IMapper mapper) : IMilestoneClient
    {
        private readonly IProjectApi projectApi = projectApi;
        private readonly IMapper mapper = mapper;

        public async Task<Domain.Entities.Milestone> CreateAsync(int projectId, MilestoneUpdateData updateData)
        {
            var milestoneDto = await projectApi.CreateMilestoneAsync(projectId, updateData);
            return mapper.Map<Domain.Entities.Milestone>(milestoneDto);
        }

        public async Task<Domain.Entities.Milestone> RemoveAsync(int milestoneId)
        {
            var milestoneDto = await projectApi.DeleteMilestoneAsync(milestoneId);
            return mapper.Map<Domain.Entities.Milestone>(milestoneDto);
        }

        public async Task<Domain.Entities.Milestone> UpdateAsync(int milestoneId, MilestoneUpdateData updateData)
        {
            var milestoneDto = await projectApi.UpdateMilestoneAsync(milestoneId, updateData);
            return mapper.Map<Domain.Entities.Milestone>(milestoneDto);
        }
    }
}

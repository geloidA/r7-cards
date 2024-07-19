using AutoMapper;
using Onlyoffice.Api.Logics.Repository;
using Onlyoffice.Api.Models;
using Onlyoffice.Api.Models.Common;

namespace Cardmngr.Application.Clients.Milestone
{
    public class MilestoneClient(IMilestoneRepository milestoneRepository, IMapper mapper) : IMilestoneClient
    {
        public async Task<Domain.Entities.Milestone> CreateAsync(int projectId, MilestoneUpdateData updateData)
        {
            var milestoneDto = await milestoneRepository.CreateAsync(projectId, updateData);
            return mapper.Map<Domain.Entities.Milestone>(milestoneDto);
        }

        public async Task<Domain.Entities.Milestone> GetAsync(int entityId)
        {
            var milestoneDto = await milestoneRepository.GetByIdAsync(entityId);
            return mapper.Map<Domain.Entities.Milestone>(milestoneDto);
        }

        public IAsyncEnumerable<Domain.Entities.Milestone> GetEntitiesAsync(FilterBuilder? filterBuilder = null)
        {
            throw new NotImplementedException();
        }

        public async Task<Domain.Entities.Milestone> RemoveAsync(int milestoneId)
        {
            var milestoneDto = await milestoneRepository.DeleteAsync(milestoneId);
            return mapper.Map<Domain.Entities.Milestone>(milestoneDto);
        }

        public async Task<Domain.Entities.Milestone> UpdateAsync(int milestoneId, MilestoneUpdateData updateData)
        {
            var milestoneDto = await milestoneRepository.UpdateAsync(milestoneId, updateData);
            return mapper.Map<Domain.Entities.Milestone>(milestoneDto);
        }

        public async Task<Domain.Entities.Milestone> UpdateStatusAsync(int id, int status)
        {
            if (status < 0 || status > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(status));
            }
            
            var milestoneDto = await milestoneRepository.UpdateStatusAsync(id, status);
            return mapper.Map<Domain.Entities.Milestone>(milestoneDto);
        }
    }
}

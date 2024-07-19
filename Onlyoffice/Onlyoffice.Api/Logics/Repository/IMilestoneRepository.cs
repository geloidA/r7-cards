using Onlyoffice.Api.Models;

namespace Onlyoffice.Api.Logics.Repository;

public interface IMilestoneRepository
{
    IAsyncEnumerable<MilestoneDto> GetAllByProjectIdAsync(int projectId);
    Task<MilestoneDto> UpdateAsync(int id, MilestoneUpdateData state);
    Task<MilestoneDto> CreateAsync(int projectId, MilestoneUpdateData state);
    Task<MilestoneDto> UpdateStatusAsync(int id, int status);
    Task<MilestoneDto> GetByIdAsync(int id);
    Task<MilestoneDto> DeleteAsync(int id);
}

using Onlyoffice.Api.Common;
using Onlyoffice.Api.Models;

namespace Onlyoffice.Api.Logics.Repository;

public interface IMilestoneRepository
{
    IAsyncEnumerable<MilestoneDto> GetAllByProjectIdAsync(int projectId);
    Task<MilestoneDto> UpdateAsync(int id, MilestoneUpdateData state);
    Task<MilestoneDto> CreateAsync(int projectId, MilestoneUpdateData state);
    Task UpdateStatusAsync(int id, Status status);
    Task<MilestoneDto> GetByIdAsync(int id);
    Task<MilestoneDto> DeleteAsync(int id);
}

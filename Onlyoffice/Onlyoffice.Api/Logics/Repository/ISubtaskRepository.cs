using Onlyoffice.Api.Common;
using Onlyoffice.Api.Models;

namespace Onlyoffice.Api.Logics.Repository;

public interface ISubtaskRepository
{
    Task<SubtaskDto> UpdateStatusAsync(int taskId, int id, Status status);
    Task<SubtaskDto> CreateAsync(int taskId, string title, string? responsible = null);
    Task<SubtaskDto> UpdateAsync(int taskId, int id, SubtaskUpdateData state);
    Task<SubtaskDto> DeleteAsync(int taskId, int id);
}

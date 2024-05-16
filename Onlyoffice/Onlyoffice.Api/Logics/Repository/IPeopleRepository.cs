using Onlyoffice.Api.Models;

namespace Onlyoffice.Api.Logics.Repository;

public interface IPeopleRepository
{
    Task<UserProfileDto?> GetByIdAsync(string userId);
    IAsyncEnumerable<UserProfileDto> GetFiltredAsync(FilterBuilder builder);
    IAsyncEnumerable<UserProfileDto> GetAllAsync();
}

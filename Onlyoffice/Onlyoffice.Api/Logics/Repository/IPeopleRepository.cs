using Onlyoffice.Api.Models;
using Onlyoffice.Api.Models.Common;

namespace Onlyoffice.Api.Logics.Repository;

public interface IPeopleRepository : IRepository<UserProfileDto>
{
    Task<UserProfileDto?> GetByIdAsync(string userId);
    IAsyncEnumerable<UserProfileDto> GetFiltredAsync(FilterBuilder builder);
}

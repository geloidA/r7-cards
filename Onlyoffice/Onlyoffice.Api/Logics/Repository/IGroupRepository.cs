using Onlyoffice.Api.Models;

namespace Onlyoffice.Api.Logics.Repository;

public interface IGroupRepository
{
    IAsyncEnumerable<GroupDto> GetAllAsync();
    IAsyncEnumerable<GroupDto> GetFiltredAsync(FilterBuilder builder);
}

using Onlyoffice.Api.Models;
using Onlyoffice.Api.Models.Common;

namespace Onlyoffice.Api.Logics.Repository;

public interface IGroupRepository : IRepository<GroupDto>
{
    IAsyncEnumerable<GroupDto> GetFiltredAsync(FilterBuilder builder);
}

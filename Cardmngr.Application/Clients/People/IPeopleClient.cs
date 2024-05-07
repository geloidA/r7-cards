using Cardmngr.Domain.Entities;
using Onlyoffice.Api;

namespace Cardmngr.Application.Clients.People;

public interface IPeopleClient
{
    IAsyncEnumerable<UserProfile> GetUsersAsync();
    IAsyncEnumerable<UserProfile> GetFilteredUsersAsync(FilterBuilder filterBuilder);
}

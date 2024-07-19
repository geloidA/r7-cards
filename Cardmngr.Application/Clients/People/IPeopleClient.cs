using Cardmngr.Domain.Entities;
using Onlyoffice.Api.Models.Common;

namespace Cardmngr.Application.Clients.People;

public interface IPeopleClient
{
    IAsyncEnumerable<UserProfile> GetUsersAsync();
    IAsyncEnumerable<UserProfile> GetFilteredUsersAsync(FilterBuilder filterBuilder);
    Task<UserProfile> GetUserProfileByIdAsync(string userId);
}

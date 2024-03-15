using Cardmngr.Domain.Entities;

namespace Cardmngr.Application.Clients.People;

public interface IPeopleClient
{
    IAsyncEnumerable<UserProfile> GetUsersAsync();
}

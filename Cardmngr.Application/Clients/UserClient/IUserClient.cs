using Cardmngr.Domain.Entities;

namespace Cardmngr.Application.Clients;

public interface IUserClient
{
    /// <exception cref="Exceptions.NoSuchUserByIdException">
    Task<UserProfile> GetUserProfileByIdAsync(string userId);
}

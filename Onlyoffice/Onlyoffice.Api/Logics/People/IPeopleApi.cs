using Onlyoffice.Api.Models;

namespace Onlyoffice.Api.Logics.People;

public interface IPeopleApi
{
    Task<UserProfileDto?> GetProfileByIdAsync(string userId);
    IAsyncEnumerable<UserProfileDto> GetUsersAsync();
}

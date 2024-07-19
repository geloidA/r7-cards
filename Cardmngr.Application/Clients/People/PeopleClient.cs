using AutoMapper;
using Cardmngr.Application.Exceptions;
using Cardmngr.Domain.Entities;
using Onlyoffice.Api.Logics.Repository;
using Onlyoffice.Api.Models.Common;

namespace Cardmngr.Application.Clients.People;

public class PeopleClient(IPeopleRepository peopleApi, IMapper mapper) : IPeopleClient
{
    public IAsyncEnumerable<UserProfile> GetFilteredUsersAsync(FilterBuilder filterBuilder)
    {
        return peopleApi.GetFilteredAsync(filterBuilder).Select(mapper.Map<UserProfile>);
    }

    public async Task<UserProfile> GetUserProfileByIdAsync(string userId)
    {
        var profile = await peopleApi.GetByIdAsync(userId) 
                      ?? throw new NoSuchUserByIdException(userId);

        return mapper.Map<UserProfile>(profile);
    }

    public IAsyncEnumerable<UserProfile> GetUsersAsync()
    {
        return peopleApi.GetAllAsync().Select(mapper.Map<UserProfile>);
    }
}

using AutoMapper;
using Cardmngr.Application.Exceptions;
using Cardmngr.Domain.Entities;
using Onlyoffice.Api.Logics.People;

namespace Cardmngr.Application.Clients;

public class UserClient(IPeopleApi peopleApi, IMapper mapper) : IUserClient
{
    private readonly IPeopleApi peopleApi = peopleApi;
    private readonly IMapper mapper = mapper;

    public async Task<UserProfile> GetUserProfileByIdAsync(string userId)
    {
        var profile = await peopleApi.GetProfileByIdAsync(userId) 
            ?? throw new NoSuchUserByIdException(userId);

        return mapper.Map<UserProfile>(profile);
    }
}

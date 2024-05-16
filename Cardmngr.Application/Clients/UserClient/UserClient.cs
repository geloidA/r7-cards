using AutoMapper;
using Cardmngr.Application.Exceptions;
using Cardmngr.Domain.Entities;
using Onlyoffice.Api.Logics.Repository;

namespace Cardmngr.Application.Clients;

public class UserClient(IPeopleRepository peopleApi, IMapper mapper) : IUserClient
{
    private readonly IPeopleRepository peopleApi = peopleApi;
    private readonly IMapper mapper = mapper;

    public async Task<UserProfile> GetUserProfileByIdAsync(string userId)
    {
        var profile = await peopleApi.GetByIdAsync(userId) 
            ?? throw new NoSuchUserByIdException(userId);

        return mapper.Map<UserProfile>(profile);
    }
}

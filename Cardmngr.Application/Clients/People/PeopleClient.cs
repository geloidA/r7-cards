using AutoMapper;
using Cardmngr.Domain.Entities;
using Onlyoffice.Api.Logics.People;

namespace Cardmngr.Application.Clients.People;

public class PeopleClient(IPeopleApi peopleApi, IMapper mapper) : IPeopleClient
{
    private readonly IPeopleApi peopleApi = peopleApi;
    private readonly IMapper mapper = mapper;

    public async IAsyncEnumerable<UserProfile> GetUsersAsync()
    {
        await foreach (var user in peopleApi.GetUsersAsync())
        {
            yield return mapper.Map<UserProfile>(user);
        }
    }
}

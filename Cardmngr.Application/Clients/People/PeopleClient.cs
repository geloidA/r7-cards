using AutoMapper;
using Cardmngr.Domain.Entities;
using Onlyoffice.Api.Logics.People;

namespace Cardmngr.Application.Clients.People;

public class PeopleClient(IPeopleApi peopleApi, IMapper mapper) : IPeopleClient
{
    private readonly IPeopleApi peopleApi = peopleApi;
    private readonly IMapper mapper = mapper;

    public IAsyncEnumerable<UserProfile> GetUsersAsync()
    {
        return peopleApi.GetUsersAsync().Select(mapper.Map<UserProfile>);
    }
}

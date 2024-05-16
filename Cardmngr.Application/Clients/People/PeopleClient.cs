using AutoMapper;
using Cardmngr.Domain.Entities;
using Onlyoffice.Api;
using Onlyoffice.Api.Logics.Repository;

namespace Cardmngr.Application.Clients.People;

public class PeopleClient(IPeopleRepository peopleApi, IMapper mapper) : IPeopleClient
{
    private readonly IPeopleRepository peopleApi = peopleApi;
    private readonly IMapper mapper = mapper;

    public IAsyncEnumerable<UserProfile> GetFilteredUsersAsync(FilterBuilder filterBuilder)
    {
        return peopleApi.GetFiltredAsync(filterBuilder).Select(mapper.Map<UserProfile>);
    }

    public IAsyncEnumerable<UserProfile> GetUsersAsync()
    {
        return peopleApi.GetAllAsync().Select(mapper.Map<UserProfile>);
    }
}

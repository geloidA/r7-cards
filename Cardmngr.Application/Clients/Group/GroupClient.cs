using AutoMapper;
using Onlyoffice.Api.Logics.Group;

namespace Cardmngr.Application.Group;

public class GroupClient(IGroupApi groupApi, IMapper mapper) : IGroupClient
{
    public IAsyncEnumerable<Domain.Entities.Group> GetGroupsAsync() => groupApi
        .GetEntitiesAsync()
        .Select(mapper.Map<Domain.Entities.Group>);
}

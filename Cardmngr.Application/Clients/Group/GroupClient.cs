﻿using AutoMapper;
using Onlyoffice.Api.Logics.Group;
using Onlyoffice.Api.Logics.Repository;

namespace Cardmngr.Application.Group;

public class GroupClient(IGroupRepository groupApi, IMapper mapper) : IGroupClient
{
    public IAsyncEnumerable<Domain.Entities.Group> GetGroupsAsync() => groupApi
        .GetAllAsync()
        .Select(mapper.Map<Domain.Entities.Group>);
}
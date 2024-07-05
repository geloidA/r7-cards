using AutoMapper;
using Cardmngr.Domain.Entities;
using Newtonsoft.Json;
using Onlyoffice.Api.Models;

namespace Cardmngr.Application.Mappers.Resolvers;

public class FeedResolver : IValueResolver<FeedDto, Feed, Domain.Entities.FeedInfo>
{
    public Domain.Entities.FeedInfo Resolve(FeedDto source, Feed destination, Domain.Entities.FeedInfo destMember, ResolutionContext context)
    {
        ArgumentNullException.ThrowIfNull(source.Feed, nameof(source.Feed));

        return JsonConvert.DeserializeObject<Domain.Entities.FeedInfo>(source.Feed)
            ?? throw new InvalidCastException("Failed to deserialize FeedInfo");
    }
}

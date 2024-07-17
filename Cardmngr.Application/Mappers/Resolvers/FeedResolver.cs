using System.Text.Json;
using AutoMapper;
using Cardmngr.Domain.Entities;
using Onlyoffice.Api.Models;

namespace Cardmngr.Application.Mappers.Resolvers;

public class FeedResolver : IValueResolver<FeedDto, Feed, FeedInfo>
{
    public FeedInfo Resolve(FeedDto source, Feed destination, FeedInfo destMember, ResolutionContext context)
    {
        ArgumentNullException.ThrowIfNull(source.Feed, nameof(source.Feed));

        return JsonSerializer.Deserialize<FeedInfo>(source.Feed)
            ?? throw new InvalidCastException("Failed to deserialize FeedInfo");
    }
}

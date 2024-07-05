using Cardmngr.Application.Clients;
using Cardmngr.Application.Clients.Feed;
using Cardmngr.Domain.Entities;
using KolBlazor;
using Microsoft.AspNetCore.Components;
using Onlyoffice.Api.Models.Common;

namespace Cardmngr.Components.FeedAggregate;

public partial class FeedView : KolComponentBase
{
    private bool _loading = true;
    private List<IGrouping<string, Feed>> _feedsByProject = [];
    private readonly Dictionary<string, UserInfo> _feedUsers = [];

    [Inject] IFeedClient FeedClient { get; set; } = default!;
    [Inject] IUserClient UserClient { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        var feeds = await FeedClient
            .GetFiltredAsync(FeedFilterBuilder.Instance.Product("projects"))
            .ToListAsync();

        _feedsByProject = feeds
            .GroupBy(x => x.Value.ExtraLocation)
            .ToList();

        await GetAllFacedUsersInFeeds(feeds);

        _loading = false;
    }

    private async Task GetAllFacedUsersInFeeds(List<Feed> feeds)
    {
        var userIds = feeds
            .SelectMany(x => (IEnumerable<Feed>)[.. x.GroupedFeeds, x])
            .Select(x => x.Value.AuthorId.ToString())
            .Distinct();
        
        foreach (var user in userIds)
        {
            _feedUsers[user] = await UserClient
                .GetUserProfileByIdAsync(user)
                .ConfigureAwait(false);
        }
    }
}

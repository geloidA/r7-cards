using Cardmngr.Application.Clients;
using Cardmngr.Application.Clients.Feed;
using Cardmngr.Application.Clients.People;
using Cardmngr.Domain.Entities;
using Cardmngr.Services;
using KolBlazor;
using Microsoft.AspNetCore.Components;
using Onlyoffice.Api.Models.Common;

namespace Cardmngr.Components.FeedAggregate;

public partial class FeedView : KolComponentBase, IDisposable
{
    private bool _loading = true;
    private List<Feed> _allFeeds = default!;
    private List<IGrouping<string, Feed>> _feedsByProject = [];
    private readonly Dictionary<string, UserInfo> _feedUsers = [];

    [Inject] private IFeedClient FeedClient { get; set; } = default!;
    [Inject] private IPeopleClient UserClient { get; set; } = default!;
    [Inject] private IFeedFilterService FeedFilterService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _allFeeds = await FeedClient
            .GetFiltredAsync(FeedFilterBuilder.Instance.Product("projects"))
            .Where(x => x.Value.Action != 2) // remove task's comment notifications
            .ToListAsync().ConfigureAwait(false);

        _feedsByProject = GetFilteredFeeds();

        FeedFilterService.FilterChanged += UpdateFilteredFeeds;

        await GetAllFacedUsersInFeeds(_allFeeds).ConfigureAwait(false);

        _loading = false;
    }

    private List<IGrouping<string, Feed>> GetFilteredFeeds()
    {
        return _allFeeds
            .Where(FeedFilterService.Filter)
            .GroupBy(x => x.Value.ExtraLocation)
            .ToList();
    }

    private void UpdateFilteredFeeds()
    {
        _feedsByProject = GetFilteredFeeds();
        StateHasChanged();
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

    public void Dispose()
    {
        FeedFilterService.FilterChanged -= UpdateFilteredFeeds;
    }
}

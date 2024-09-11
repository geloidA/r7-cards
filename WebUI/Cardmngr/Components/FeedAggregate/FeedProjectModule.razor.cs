using Cardmngr.Domain.Entities;
using Cardmngr.Services;
using KolBlazor;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.FeedAggregate;

public partial class FeedProjectModule : KolComponentBase
{
    private string? _projectUrl;

    [Parameter, EditorRequired] 
    public string Name { get; set; } = "";

    [Parameter, EditorRequired]
    public IEnumerable<Feed> Feeds { get; set; } = null!;

    private FeedFilterService _feedFilterService = default!;

    [Inject] IFeedFilterService FeedFilterService { get; set; } = default!;

    protected override void OnInitialized()
    {
        _feedFilterService = (FeedFilterService)FeedFilterService;

        if (Feeds.Any())
        {
            var extraUrl = Feeds.First().Value.ExtraLocationUrl;
            var idStartIndex = extraUrl.LastIndexOf('=') + 1;
            _projectUrl = $"/project/board?ProjectId={extraUrl[idStartIndex..]}";
        }
    }
}

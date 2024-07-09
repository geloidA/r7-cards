using Cardmngr.Domain.Entities;
using Cardmngr.Services;
using KolBlazor;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.FeedAggregate;

public partial class FeedProjectModule : KolComponentBase
{
    [Parameter, EditorRequired] 
    public string Name { get; set; } = "";

    [Parameter, EditorRequired]
    public IEnumerable<Feed> Feeds { get; set; } = null!;

    private FeedFilterService _feedFilterService = default!;

    [Inject] IFeedFilterService FeedFilterService { get; set; } = default!;

    protected override void OnInitialized()
    {
        _feedFilterService = (FeedFilterService)FeedFilterService;
    }
}

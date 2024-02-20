using Cardmngr.Application.Clients.FeedbackClient;
using Cardmngr.Domain.Feedback;
using Cardmngr.Shared.Feedbacks;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.FeedbackAggregate;

public partial class FeedbacksState
{
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Inject] public IFeedbackClient FeedbackClient { get; set; } = default!;

    public FeedbacksVm? Model { get; set; }
    public bool Initialized { get; private set; }

    protected override async Task OnInitializedAsync()
    {
        Model = await FeedbackClient.GetFeedbacksAsync();
        Initialized = true;
    }

    internal async Task CreateFeedbackAsync(FeedbackUpdateData data)
    {
        var created = await FeedbackClient.CreateFeedbackAsync(data);
        Model!.Feedbacks.Add(created);
    }
}

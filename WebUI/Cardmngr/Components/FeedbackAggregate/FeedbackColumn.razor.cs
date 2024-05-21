using Cardmngr.Domain.Enums;
using Cardmngr.Domain.Feedback;
using KolBlazor.Components;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.FeedbackAggregate;

public partial class FeedbackColumn
{
    private IList<Feedback> Items => [.. State.Model!.Feedbacks
        .Where(x => x.Status == Status)
        .OrderByDescending(x => x.LikedUsers.Count)
        .ThenBy(x => x.DislikedUsers.Count)];

    [Parameter] public RenderFragment? HeaderTemplate { get; set; }
    [Parameter] public int Width { get; set; } = 324;
    [Parameter, EditorRequired] public FeedbackStatus Status { get; set; }
    [CascadingParameter] FeedbacksState State { get; set; } = null!;

    private async Task DropFeedback(Feedback dropped)
    {
        if (dropped.Status == Status) return;
        await State.UpdateFeedbackStatusAsync(dropped.Id, Status);
    }    
}

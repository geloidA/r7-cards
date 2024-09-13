using Blazored.Modal;
using Blazored.Modal.Services;
using Cardmngr.Components.FeedbackAggregate.Modals;
using Cardmngr.Domain.Entities.Feedback;
using Microsoft.AspNetCore.Components;
using Onlyoffice.Api.Extensions;

namespace Cardmngr.Components.FeedbackAggregate;

public partial class FeedbackCard
{
    private bool likePressed;
    private bool dislikePressed;

    [CascadingParameter] FeedbacksState State { get; set; } = null!;
    [Parameter] public Feedback Feedback { get; set; } = null!;    
    [CascadingParameter(Name = "DetailsModal")] ModalOptions DetailsModal { get; set; } = null!;
    [CascadingParameter] IModalService Modal { get; set; } = null!;

    protected override void OnInitialized()
    {
        openModalFunc = OpenModal;
        var userId = AuthenticationStateProvider.ToCookieProvider().UserId;
        likePressed = Feedback.LikedUsers.Contains(userId);
        dislikePressed = Feedback.DislikedUsers.Contains(userId);
    }

    Func<Task> openModalFunc = null!;

    private async Task OpenModal()
    {
        var parameters = new ModalParameters
        {
            { "State", State },
            { "Model", Feedback }
        };

        await Modal.Show<FeedbackDetailsModal>(parameters, DetailsModal).Result;
    }

    private async Task ToggleLike()
    {
        likePressed = !likePressed;
        dislikePressed = false;

        await State.ToggleFeedbackLikeAsync(Feedback.Id);
    }

    private async Task ToggleDislike()
    {
        dislikePressed = !dislikePressed;
        likePressed = false;

        await State.ToggleFeedbackDislikeAsync(Feedback.Id);
    }
}

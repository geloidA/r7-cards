using Cardmngr.Components.Modals.Base;
using Cardmngr.Domain.Entities.Feedback;
using Cardmngr.Shared.Feedbacks;
using Cardmngr.Shared.Utils.Comparer;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.FeedbackAggregate.Modals;

public partial class FeedbackDetailsModal() : AddEditModalBase<Feedback, FeedbackUpdateData>(new FeedbackFeedbackUpdateDataEqualityComparer())
{
    Components.Modals.MyBlazored.Offcanvas currentModal = null!;

    private bool CanEdit => Model == null || Model.CanEdit;

    [Parameter] public FeedbacksState State { get; set; } = null!;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (IsAdd)
        {
            Buffer.Title = "Название";
        }
    }

    private async Task SubmitAsync()
    {
        if (enterPressed)
        {
            enterPressed = false;
            return;
        }

        if (IsAdd)
        {
            await State.CreateFeedbackAsync(Buffer);
        }
        else
        {
            await State.UpdateFeedbackAsync(Model!.Id, Buffer);
        }

        SkipConfirmation = true;
        await currentModal.CloseAsync();
    }

    private async Task DeleteAsync() 
    {
        var answer = await ShowDeleteConfirm("Удаление предложения");
        
        if (answer.Confirmed)
        {
            await State.RemoveFeedbackAsync(Model!.Id);

            SkipConfirmation = true;
            await currentModal.CloseAsync();
        }
    }
}

using Cardmngr.Components.Modals.Base;
using Cardmngr.Domain.Feedback;
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
            buffer.Title = "Название";
        }
    }

    private async Task SubmitAsync()
    {
        if (IsAdd)
        {
            await State.CreateFeedbackAsync(buffer);
        }
        else
        {
            await State.UpdateFeedbackAsync(Model!.Id, buffer);
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

using Cardmngr.Components.Common;
using Cardmngr.Components.Modals.Base;
using Cardmngr.Domain.Feedback;
using Cardmngr.Shared.Feedbacks;
using Cardmngr.Utils.DetailsModal;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.FeedbackAggregate.Modals;

public partial class FeedbackDetailsModal : AddEditModalBase<Feedback, FeedbackUpdateData>
{
    Components.Modals.MyBlazored.Offcanvas currentModal = null!;

    private TitleChanger titleChanger = null!;

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

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await Task.Delay(1); // wait for correct changer toggle
            await titleChanger.ToggleEditMode();
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

        await currentModal.CloseAsync();
    }

    private async Task DeleteAsync() 
    {
        var answer = await ShowDeleteConfirm("Удаление предложения");
        
        if (answer.Confirmed)
        {
            await State.RemoveFeedbackAsync(Model!.Id);
            await currentModal.CloseAsync();
        }
    }
}

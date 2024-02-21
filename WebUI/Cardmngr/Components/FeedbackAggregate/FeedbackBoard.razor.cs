using Blazored.Modal;
using Blazored.Modal.Services;
using Cardmngr.Components.FeedbackAggregate.Modals;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.FeedbackAggregate;

public partial class FeedbackBoard
{
    [CascadingParameter] FeedbacksState State { get; set; } = null!;
    [CascadingParameter] IModalService Modal { get; set; } = default!;
    [CascadingParameter(Name = "DetailsModal")] ModalOptions ModalOptions { get; set; } = null!;

    protected override void OnInitialized()
    {
        State.StateChanged += StateHasChanged;
    }

    private async Task OpenCreateModalAsync()
    {
        var parameters = new ModalParameters
        {
            { "State", State },
            { "IsAdd", true }
        };

        await Modal.Show<FeedbackDetailsModal>("", parameters, ModalOptions).Result;
    }
}

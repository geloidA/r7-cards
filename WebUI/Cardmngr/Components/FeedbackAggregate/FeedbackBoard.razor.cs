using Microsoft.AspNetCore.Components;
using Onlyoffice.Api.Logics.People;

namespace Cardmngr.Components.FeedbackAggregate;

public partial class FeedbackBoard
{
    [CascadingParameter] FeedbacksState State { get; set; } = null!;
    [Inject] IPeopleApi PeopleApi { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await State.CreateFeedbackAsync(new Shared.Feedbacks.FeedbackUpdateData { Title = "Bob" });
        await PeopleApi.GetProfileByIdAsync("Administrator");
    }
}

using BlazorBootstrap;
using Cardmngr.Application.Clients.FeedbackClient;
using Cardmngr.Domain.Entities;
using Cardmngr.Domain.Enums;
using Cardmngr.Shared.Feedbacks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Onlyoffice.Api.Providers;

namespace Cardmngr.Components.FeedbackAggregate;

public partial class FeedbacksState : ComponentBase
{
    private UserInfo? currentUser;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

    [Inject] ToastService ToastService { get; set; } = default!;

    [Inject] IFeedbackClient FeedbackClient { get; set; } = default!;

    public FeedbacksVm? Model { get; set; }
    public bool Initialized { get; private set; }

    public event Action? StateChanged;
    private void OnStateChanged() => StateChanged?.Invoke();

    protected override async Task OnInitializedAsync()
    {
        currentUser = AuthenticationStateProvider.ToCookieProvider().UserInfo;

        if (currentUser == null)
        {
            throw new NullReferenceException("User is not logged in");
        }

        Model = await FeedbackClient.GetFeedbacksAsync();
        Initialized = true;
    }

    internal async Task CreateFeedbackAsync(FeedbackUpdateData data)
    {
        var created = await FeedbackClient.CreateFeedbackAsync(new FeedbackCreateRequestData(currentUser!, data));
        Model!.Feedbacks.Add(created);
        OnStateChanged();
    }

    internal async Task RemoveFeedbackAsync(int feedbackId)
    {
        var deleted = await FeedbackClient.DeleteFeedbackAsync(feedbackId);

        if (deleted is { })
        {
            Model!.Feedbacks.RemoveAll(x => x.Id == deleted.Id);
            OnStateChanged();
        }
        else
        {
            ToastService.Notify(new ToastMessage(ToastType.Danger, "Не удалось удалить"));
        }
    }

    internal async Task UpdateFeedbackAsync(int feedbackId, FeedbackUpdateData data)
    {
        var updated = await FeedbackClient.UpdateFeedbackAsync(feedbackId, data);

        if (updated is { })
        {
            Model!.Feedbacks.RemoveAll(x => x.Id == feedbackId);
            Model.Feedbacks.Add(updated);
            OnStateChanged();
        }
        else
        {
            ToastService.Notify(new ToastMessage(ToastType.Danger, "Не удалось обновить"));
        }
    }

    internal async Task ToggleFeedbackLikeAsync(int feedbackId)
    {
        var updated = await FeedbackClient.ToggleFeedbackLikeAsync(feedbackId);

        if (updated is { })
        {
            Model!.Feedbacks.RemoveAll(x => x.Id == feedbackId);
            Model.Feedbacks.Add(updated);
            OnStateChanged();
        }
        else
        {
            ToastService.Notify(new ToastMessage(ToastType.Danger, "Не удалось обновить"));
        }
    }

    internal async Task UpdateFeedbackStatusAsync(int feedbackId, FeedbackStatus status)
    {
        var updated = await FeedbackClient.UpdateFeedbackStatusAsync(feedbackId, status);

        Model!.Feedbacks.RemoveAll(x => x.Id == feedbackId);
        Model.Feedbacks.Add(updated);
        OnStateChanged();
    }
}

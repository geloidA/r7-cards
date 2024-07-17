using System.Net.Http.Json;
using Cardmngr.Domain.Entities.Feedback;
using Cardmngr.Domain.Enums;
using Cardmngr.Shared.Feedbacks;
using Microsoft.AspNetCore.Components.Authorization;
using Onlyoffice.Api.Extensions;

namespace Cardmngr.Application.Clients.FeedbackClient;

public class FeedbackClient(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider) : IFeedbackClient
{
    private readonly string userGuid = authenticationStateProvider.ToCookieProvider().UserId;
    private readonly HttpClient httpClient = httpClient;

    public async Task<Feedback> CreateFeedbackAsync(FeedbackCreateRequestData data)
    {
        var response = await httpClient.PostAsJsonAsync($"/api/feedback", data);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Feedback>() 
            ?? throw new NullReferenceException("Something went wrong while creating feedback");
    }

    public async Task<Feedback?> DeleteFeedbackAsync(int feedbackId)
    {
        var response = await httpClient.DeleteAsync($"/api/feedback/{feedbackId}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Feedback>();
        }

        return null;
    }

    public async Task<FeedbacksVm> GetFeedbacksAsync()
    {
        var response = await httpClient.GetAsync($"/api/feedback/all/{userGuid}");

        response.EnsureSuccessStatusCode();

        var feedbacks = await response.Content.ReadFromJsonAsAsyncEnumerable<Feedback>().ToListAsync();
        return feedbacks == null
            ? throw new NullReferenceException("Something went wrong while getting feedbacks")
            : new FeedbacksVm { Feedbacks = feedbacks! };
    }

    public async Task<Feedback?> ToggleFeedbackDislikeAsync(int feedbackId)
    {
        var response = await httpClient.PostAsync($"/api/feedback/dislike/{feedbackId}/{userGuid}", null);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Feedback>()
                ?? throw new NullReferenceException("Something went wrong while toggling feedback dislike");
        }

        return null;
    }

    public async Task<Feedback?> ToggleFeedbackLikeAsync(int feedbackId)
    {
        var response = await httpClient.PostAsync($"/api/feedback/like/{feedbackId}/{userGuid}", null);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Feedback>()
                ?? throw new NullReferenceException("Something went wrong while toggling feedback like");
        }

        return null;
    }

    public async Task<Feedback?> UpdateFeedbackAsync(int feedbackId, FeedbackUpdateData data)
    {
        var response = await httpClient.PutAsJsonAsync($"/api/feedback/{userGuid}/{feedbackId}", data);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Feedback>()
                ?? throw new NullReferenceException("Can't deserialize Update feedback response");
        }

        return null;
    }

    public async Task<Feedback> UpdateFeedbackStatusAsync(int feedbackId, FeedbackStatus status)
    {
        var response = await httpClient.PutAsJsonAsync($"/api/feedback/status/{userGuid}/{feedbackId}", status);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Feedback>()
            ?? throw new NullReferenceException("Can't deserialize Update feedback response");
    }
}

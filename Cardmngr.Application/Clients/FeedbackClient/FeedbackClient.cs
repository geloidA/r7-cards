using System.Net.Http.Json;
using Cardmngr.Domain.Enums;
using Cardmngr.Domain.Feedback;
using Cardmngr.Shared.Feedbacks;
using Microsoft.AspNetCore.Components.Authorization;
using Onlyoffice.Api.Extensions;

namespace Cardmngr.Application.Clients.FeedbackClient;

public class FeedbackClient(IHttpClientFactory httpClientFactory, AuthenticationStateProvider authenticationStateProvider) : IFeedbackClient
{
    private readonly string userGuid = authenticationStateProvider.ToCookieProvider().UserId;
    private readonly IHttpClientFactory httpClientFactory = httpClientFactory;

    public async Task<Feedback> CreateFeedbackAsync(FeedbackCreateRequestData data)
    {
        using var client = httpClientFactory.CreateClient("self-api-cookie");

        var response = await client.PostAsJsonAsync($"api/feedback", data);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Feedback>() 
                ?? throw new NullReferenceException("Something went wrong while creating feedback");
    }

    public async Task<Feedback?> DeleteFeedbackAsync(int feedbackId)
    {
        using var client = httpClientFactory.CreateClient("self-api-cookie");

        var response = await client.DeleteAsync($"api/feedback/{feedbackId}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Feedback>();
        }

        return null;
    }

    public async Task<FeedbacksVm> GetFeedbacksAsync()
    {
        using var client = httpClientFactory.CreateClient("self-api-cookie");

        var response = await client.GetAsync($"api/feedback/all/{userGuid}");

        response.EnsureSuccessStatusCode();

        var feedbacks = await response.Content.ReadFromJsonAsAsyncEnumerable<Feedback>().ToListAsync();
        return feedbacks == null
            ? throw new NullReferenceException("Something went wrong while getting feedbacks")
            : new FeedbacksVm { Feedbacks = feedbacks! };
    }

    public async Task<Feedback?> ToggleFeedbackDislikeAsync(int feedbackId)
    {
        using var client = httpClientFactory.CreateClient("self-api-cookie");

        var response = await client.PostAsync($"api/feedback/dislike/{feedbackId}/{userGuid}", null);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Feedback>()
                ?? throw new NullReferenceException("Something went wrong while toggling feedback dislike");
        }

        return null;
    }

    public async Task<Feedback?> ToggleFeedbackLikeAsync(int feedbackId)
    {
        using var client = httpClientFactory.CreateClient("self-api-cookie");

        var response = await client.PostAsync($"api/feedback/like/{feedbackId}/{userGuid}", null);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Feedback>()
                ?? throw new NullReferenceException("Something went wrong while toggling feedback like");
        }

        return null;
    }

    public async Task<Feedback?> UpdateFeedbackAsync(int feedbackId, FeedbackUpdateData data)
    {
        using var client = httpClientFactory.CreateClient("self-api-cookie");

        var response = await client.PutAsJsonAsync($"api/feedback/{userGuid}/{feedbackId}", data);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Feedback>()
                ?? throw new NullReferenceException("Can't deserialize Update feedback response");
        }

        return null;
    }

    public async Task<Feedback> UpdateFeedbackStatusAsync(int feedbackId, FeedbackStatus status)
    {
        using var client = httpClientFactory.CreateClient("self-api-cookie");

        var response = await client.PutAsJsonAsync($"api/feedback/status/{userGuid}/{feedbackId}", status);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Feedback>()
            ?? throw new NullReferenceException("Can't deserialize Update feedback response");
    }
}

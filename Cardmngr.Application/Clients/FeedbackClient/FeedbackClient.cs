using System.Net.Http.Json;
using Cardmngr.Domain.Feedback;
using Cardmngr.Shared.Feedbacks;
using Microsoft.AspNetCore.Components.Authorization;
using Onlyoffice.Api.Providers;

namespace Cardmngr.Application.Clients.FeedbackClient;

public class FeedbackClient(IHttpClientFactory httpClientFactory, AuthenticationStateProvider authenticationStateProvider) : IFeedbackClient
{
    private readonly string userGuid = authenticationStateProvider.ToCookieProvider()["UserId"];
    private readonly IHttpClientFactory httpClientFactory = httpClientFactory;

    public async Task<Feedback> CreateFeedbackAsync(FeedbackUpdateData data)
    {
        using var client = httpClientFactory.CreateClient("self-api-cookie");

        var response = await client.PostAsJsonAsync($"api/feedback/{userGuid}", data);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Feedback>() 
                ?? throw new NullReferenceException("Something went wrong while creating feedback");
        }

        throw new Exception(response.ReasonPhrase);
    }

    public async Task<FeedbacksVm> GetFeedbacksAsync()
    {
        using var client = httpClientFactory.CreateClient("self-api-cookie");

        var response = await client.GetAsync($"api/feedback/all/{userGuid}");

        if (response.IsSuccessStatusCode)
        {
            var feedbacks = await response.Content.ReadFromJsonAsync<List<Feedback>>()
                ?? throw new NullReferenceException("Something went wrong while getting feedbacks");

            return new FeedbacksVm { Feedbacks = feedbacks };
        }

        throw new Exception(response.ReasonPhrase);
    }
}

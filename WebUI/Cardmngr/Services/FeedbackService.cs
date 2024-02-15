using System.Net;
using System.Net.Http.Json;
using Onlyoffice.Api;

namespace Cardmngr.Services;

public interface IFeedbackService
{
    Task<HttpStatusCode> SendAsync(Feedback feedback);
}

public class FeedbackService(IHttpClientFactory httpClientFactory) : ApiLogicBase(httpClientFactory), IFeedbackService
{
    public async Task<HttpStatusCode> SendAsync(Feedback feedback)
    {
        var response = await InvokeHttpClientAsync(c => c.PostAsJsonAsync($"api/feedback", feedback), "self-api");
        return response.StatusCode;
    }
}

public class Feedback(string userName, string text)
{
    public string UserName { get; } = userName;
    public string Text { get; } = text;
    public DateTime CreatedAt { get; } = DateTime.Now;
}

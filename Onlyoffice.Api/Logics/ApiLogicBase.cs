namespace Onlyoffice.Api;

public class ApiLogicBase(IHttpClientFactory httpClientFactory)
{
    protected readonly IHttpClientFactory httpClientFactory = httpClientFactory;

    protected async Task<T> InvokeHttpClientAsync<T>(Func<HttpClient, Task<T>> func, string apiUrl = "onlyoffice")
    {
        using var client = httpClientFactory.CreateClient(apiUrl);
        return await func(client);
    }

    protected async Task InvokeHttpClientAsync(Func<HttpClient, Task> func, string apiUrl = "onlyoffice")
    {
        using var client = httpClientFactory.CreateClient(apiUrl);
        await func(client);
    }
}

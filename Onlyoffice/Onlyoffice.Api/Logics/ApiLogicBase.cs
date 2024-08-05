namespace Onlyoffice.Api;

public class ApiLogicBase(IHttpClientFactory httpClientFactory)
{
    protected readonly IHttpClientFactory HttpClientFactory = httpClientFactory;

    protected async Task<T> InvokeHttpClientAsync<T>(Func<HttpClient, Task<T>> func, string apiUrl = "onlyoffice")
    {
        using var client = HttpClientFactory.CreateClient(apiUrl);
        return await func(client);
    }

    protected async Task InvokeHttpClientAsync(Func<HttpClient, Task> func, string apiUrl = "onlyoffice")
    {
        using var client = HttpClientFactory.CreateClient(apiUrl);
        await func(client);
    }
}

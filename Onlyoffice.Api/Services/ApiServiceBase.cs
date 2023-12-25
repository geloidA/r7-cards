namespace Onlyoffice.Api.Services;

public abstract class ApiServiceBase : IDisposable
{
    protected readonly string apiUrl;
    protected readonly HttpClient httpClient;

    public ApiServiceBase(HttpClient httpClient, string apiUrl)
    {
        if (string.IsNullOrWhiteSpace(apiUrl)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(apiUrl));
        this.apiUrl = apiUrl;        
        this.httpClient = httpClient;
    }

    public void Dispose()
    {
        httpClient.Dispose();
    }
}

namespace Onlyoffice.Api;

public class ApiLogicBase(IHttpClientFactory httpClientFactory)
{
    protected readonly IHttpClientFactory httpClientFactory = httpClientFactory;
}

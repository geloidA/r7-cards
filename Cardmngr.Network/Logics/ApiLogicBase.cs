namespace Cardmngr.Network;

public class ApiLogicBase(IHttpClientFactory httpClientFactory)
{
    protected readonly IHttpClientFactory httpClientFactory = httpClientFactory;
}

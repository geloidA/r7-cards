using Microsoft.AspNetCore.Mvc;

namespace Onlyoffice.ProxyServer.Controllers;

public class PeopleController(IConfiguration conf) : ApiController(conf)
{
    [Route("api/[controller]/@self")]
    public Task ProxySelf() => ProxyRequestAsync($"{apiUrl}/people/@self");

    [Route("api/[controller]/storage/userPhotos/{**rest}")]
    public Task ProxyStorageUserPhotos(string rest) => ProxyRequestAsync($"{serverUrl}/storage/userPhotos/{rest}");

    [Route("api/[controller]/skins/default/images/{**rest}")]
    public Task ProxySkinsDefaultImages(string rest) => ProxyRequestAsync($"{serverUrl}/skins/default/images/{rest}");
}

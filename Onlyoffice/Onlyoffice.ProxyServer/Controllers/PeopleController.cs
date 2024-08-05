using Microsoft.AspNetCore.Mvc;

namespace Onlyoffice.ProxyServer.Controllers;

[Route("api/[controller]")]
public class PeopleController(IConfiguration conf) : ApiController(conf)
{
    [Route("@self")]
    public Task ProxySelf() => ProxyRequestAsync($"{ApiUrl}/people/@self");

    [Route("storage/userPhotos/{**rest}")]
    public Task ProxyStorageUserPhotos(string rest) => ProxyRequestAsync($"{ServerUrl}/storage/userPhotos/{rest}");

    [Route("skins/default/images/{**rest}")]
    public Task ProxySkinsDefaultImages(string rest) => ProxyRequestAsync($"{ServerUrl}/skins/default/images/{rest}");

    [HttpGet("{id}")]
    public Task ProxyGetProfileById(string id) => ProxyRequestAsync($"{ApiUrl}/people/{id}");

    [HttpGet("filter/{**rest}")]
    public Task ProxyFilterPeople(string rest) => ProxyRequestAsync($"{ApiUrl}/people/filter?{rest}");

    [HttpGet("")]
    public Task ProxyGetUsers() => ProxyRequestAsync($"{ApiUrl}/people");
}

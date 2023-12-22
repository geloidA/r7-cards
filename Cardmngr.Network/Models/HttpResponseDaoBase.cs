namespace Cardmngr.Network.Models;

public abstract class HttpResponseDaoBase
{
    public int Count { get; set; }
    public int Status { get; set; }
    public int StatusCode { get; set; }
}

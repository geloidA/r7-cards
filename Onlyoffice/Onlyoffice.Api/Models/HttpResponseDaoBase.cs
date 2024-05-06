namespace Onlyoffice.Api.Models;

public abstract class MultiResponseDao<T> : ResponseDaoBase
{
    public List<T> Response { get; set; } = [];
}

public class SingleResponseDao<T> : ResponseDaoBase
{
    public T? Response { get; set; }
}

public class ResponseDaoBase
{
    public int Count { get; set; }
    public int Status { get; set; }
    public int StatusCode { get; set; }
}

using System.Net;

namespace Cardmngr.Domain.Net;

public class HttpResult<T>(T result = default, HttpStatusCode code = HttpStatusCode.OK) : IHttpResult<T>
{
    public T Result => result;

    public HttpStatusCode Code => code;

    public bool IsSuccess => (int)code % 200 < 27;
}

public interface IHttpResult<T>
{
    T Result { get; }

    HttpStatusCode Code { get; }

    bool IsSuccess { get; }
}
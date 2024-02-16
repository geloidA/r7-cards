using System.Net;

namespace Cardmngr.Application;

public record Result(HttpStatusCode StatusCode, IEnumerable<string>? Errors = null);

public record Result<T>(HttpStatusCode StatusCode, T Value, IEnumerable<string>? Errors = null) : Result(StatusCode, Errors);

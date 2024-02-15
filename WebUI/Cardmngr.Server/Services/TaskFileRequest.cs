namespace Cardmngr.Server.Services;

public class TaskFileRequest<TValue> : TaskFileRequest
{
    public TValue? Value { get; set; }
}

public class TaskFileRequest
{
    public string? Guid { get; set; }
}

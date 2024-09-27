namespace Cardmngr.Application.Clients.Base;

public record FilteredEntitiesResponse<TEntity>
{
    public int? Count { get; init; }
    public int? Total { get; init; }
    public int? StartIndex { get; init; }
    public int Status { get; init; }
    public int StatusCode { get; init; }
    public ICollection<TEntity> Response { get; init; } = [];
};
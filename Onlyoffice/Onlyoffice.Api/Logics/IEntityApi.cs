namespace Onlyoffice.Api;

public interface IEntityApi<TEntity>
{
    IAsyncEnumerable<TEntity> GetEntitiesAsync();

    IAsyncEnumerable<TEntity> GetEntitiesAsync(FilterBuilder filterBuilder);
    
}

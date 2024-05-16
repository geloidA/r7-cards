namespace Onlyoffice.Api.Logics.Repository;

public interface IRepository<TEntity>
{
    IAsyncEnumerable<TEntity> GetAllAsync();
}

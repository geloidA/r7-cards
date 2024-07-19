namespace Onlyoffice.Api.Logics.Repository;

public interface IRepository<out TEntity>
{
    IAsyncEnumerable<TEntity> GetAllAsync();
}

namespace Onlyoffice.Api.Models;

public interface IEntityDto<TId>
{
    TId Id { get; set; }
}

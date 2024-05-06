namespace Cardmngr.Application.Group;
using Cardmngr.Domain.Entities;

public interface IGroupClient
{
    IAsyncEnumerable<Group> GetGroupsAsync();
}

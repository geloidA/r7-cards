using Cardmngr.Application.Clients.Base;
using Onlyoffice.Api.Models;

namespace Cardmngr.Application.Clients.Milestone;

public interface IMilestoneClient : IEntityClient<Domain.Entities.Milestone, MilestoneUpdateData>
{
    /// <summary>
    /// Updates the status of a milestone
    /// </summary>
    /// <param name="id"></param>
    /// <param name="status">0 - open, 1 - closed</param>
    /// <returns></returns>
    Task<Domain.Entities.Milestone> UpdateStatusAsync(int id, int status);
}

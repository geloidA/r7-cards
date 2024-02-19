using Cardmngr.Application.Clients.Base;
using Onlyoffice.Api.Models;

namespace Cardmngr.Application.Clients.Milestone
{
    public interface IMilestoneClient : IEntityClient<Domain.Entities.Milestone, MilestoneUpdateData>
    {

    }
}

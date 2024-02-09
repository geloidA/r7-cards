using Cardmngr.Models.Base;
using Cardmngr.Models.Interfaces;
using Onlyoffice.Api.Models;

namespace Cardmngr;

public class FeedbackProject(Project project) : ProjectModelBase(project)
{
    public override IEnumerable<IUser> Team => [new User { DisplayName = "Колпаков Александр "}];

    public override IStatusColumnBoard StatusBoard => throw new NotImplementedException();

    public override IObservableCollection<IMilestoneModel> Milestones => throw new NotImplementedException();
}

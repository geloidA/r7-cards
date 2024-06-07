using Cardmngr.Domain;
using KolBlazor;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate.Dashboard;

public partial class DashboardWatchManager : KolComponentBase
{
    private bool _isPopoverOpen;
    private readonly string _popoverGuid = Guid.NewGuid().ToString();

    [Parameter, EditorRequired] public IEnumerable<ProjectInfo> Projects { get; set; } = null!;
}

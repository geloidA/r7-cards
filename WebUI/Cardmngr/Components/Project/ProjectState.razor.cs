using Cardmngr.Models;
using Microsoft.AspNetCore.Components;
using Onlyoffice.Api.Logics;

namespace Cardmngr.Components.Project;

public partial class ProjectState : ComponentBase
{
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Inject] public IProjectApi ProjectApi { get; set; } = null!;

    public ProjectStateVm? Model { get; set; }

    
}

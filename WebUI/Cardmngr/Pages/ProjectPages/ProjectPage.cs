using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Pages.ProjectPages;

[Authorize]
public abstract class ProjectPage : ComponentBase
{
    [SupplyParameterFromQuery] public int? ProjectId { get; set; }
}

using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate;

public partial class ProjectBoard : ComponentBase
{
    [CascadingParameter] Dictionary<int, int> CommonHeightByKey { get ;set; } = null!;

    [CascadingParameter] IProjectState State { get; set; } = null!;
}

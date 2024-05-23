using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate;

public partial class ProjectBoard : ComponentBase
{
    private readonly Dictionary<object, int> _commonHeightByKey = [];

    [CascadingParameter] IProjectState State { get; set; } = null!;
}

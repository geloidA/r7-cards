using Cardmngr.Components.Common;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate.Components;

public partial class ProjectProgressIndicator : ComponentBase
{
    [Parameter, EditorRequired] public ProgressState Progress { get; set; } = null!;
}

using Cardmngr.Domain.Entities;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.TaskAggregate;

public partial class DashboardTaskCard : ComponentBase
{
    [Parameter, EditorRequired] 
    public OnlyofficeTask Task { get; set; } = null!;
}

using Cardmngr.Domain.Entities;
using Cardmngr.Domain.Enums;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.TaskAggregate;

public partial class DashboardTaskCard : ComponentBase
{
    [Parameter, EditorRequired] 
    public OnlyofficeTask Task { get; set; } = null!;

    private string CssHighPriority => Task.Priority == Priority.High ? "high-priority" : "";
}

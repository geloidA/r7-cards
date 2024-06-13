using Microsoft.AspNetCore.Components;

namespace Cardmngr.Pages;

public partial class DashboardPage : ComponentBase
{
    [Parameter] public int ProjectId { get; set; }
}

using KolBlazor;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.Modals.MyBlazored;

public abstract class OffcanvasBase : KolComponentBase
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
}

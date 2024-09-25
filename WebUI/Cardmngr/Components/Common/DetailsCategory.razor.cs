using KolBlazor;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.Common;

public partial class DetailsCategory : KolComponentBase
{
    [Parameter]
    public bool Open { get; set; }

    [Parameter]
    public EventCallback<bool> OpenChanged { get; set; }

    [Parameter]
    public string? Header { get; set; }

    [Parameter]
    public RenderFragment? Content { get; set; }

    [Parameter]
    public RenderFragment<bool>? HeaderTemplate { get; set; }

    private Task ToggleOpen()
    {
        Open = !Open;
        return OpenChanged.InvokeAsync(Open);
    }
}

using KolBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Cardmngr.Components.Common;

public partial class Button : KolComponentBase
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    private string CssDisabled => Disabled ? "cursor-not-allowed opacity-50" : "cursor-pointer hover:brightness-110 active:brightness-100";

    protected override Task HandleClick(MouseEventArgs e)
    {
        if (Disabled)
        {
            return Task.CompletedTask;
        }

        return base.HandleClick(e);
    }
}

using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.Modals.MyBlazored;

public partial class Offcanvas : OffcanvasBase
{
    private string DelayedShow = "";
    private string CloseBtnRadiusStyle => Placement == MyOffcanvasPlacement.Start 
        ? "border-radius: 0px 7px 0px 0px;"
        : "border-radius: 7px 0px 0px 0px;";

    [CascadingParameter] BlazoredModalInstance CurrentModel { get; set; } = null!;

    [Parameter] public MyOffcanvasPlacement Placement { get; set; }
    [Parameter] public bool ShowCloseBtn { get; set; } = true;

    public async Task CloseAsync(ModalResult? result = null)
    {
        await ToggleDelayShowAsync();
        await CurrentModel.CloseAsync(result ?? ModalResult.Ok());
    }

    protected override async Task OnInitializedAsync()
    {
        await ToggleDelayShowAsync();
    }

    private async Task ToggleDelayShowAsync() // need for animation showing
    {
        await Task.Delay(1);
        DelayedShow = string.IsNullOrEmpty(DelayedShow) ? "show" : "";
    }

    private string CssPlacement
    {
        get
        {
            return Placement switch
            {
                MyOffcanvasPlacement.Start => "offcanvas-start",
                MyOffcanvasPlacement.End => "offcanvas-end",
                MyOffcanvasPlacement.Top => "offcanvas-top",
                MyOffcanvasPlacement.Bottom => "offcanvas-bottom",
                _ => throw new NotImplementedException()
            };
        }
    }
}

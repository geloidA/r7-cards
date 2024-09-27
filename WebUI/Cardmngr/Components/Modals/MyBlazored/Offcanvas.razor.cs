using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Cardmngr.Components.Modals.MyBlazored;

public partial class Offcanvas : OffcanvasBase
{
    private string delayedShow = "";
    private string CloseBtnRadiusStyle => Placement == MyOffcanvasPlacement.Start 
        ? "border-radius: 0px 7px 0px 0px;"
        : "border-radius: 7px 0px 0px 0px;";

    [CascadingParameter] private BlazoredModalInstance CurrentModel { get; set; } = null!;

    [Parameter] public MyOffcanvasPlacement Placement { get; set; }
    [Parameter] public bool ShowCloseBtn { get; set; } = true;
    [Parameter] public int Width { get; set; } = 450;
    [Parameter] public Func<Task<ModalResult>>? OnClose { get; set; }

    private ElementReference myOffcanvas;

    private async Task HandleKeyUp(KeyboardEventArgs e)
    {
        if (e.Key == "Escape")
        {
            await CloseAsync(ModalResult.Cancel()).ConfigureAwait(false);
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await Task.Delay(100).ConfigureAwait(false);
            await myOffcanvas.FocusAsync().ConfigureAwait(false);
        }
    }

    public async Task CloseAsync(ModalResult? result = null)
    {
        if (OnClose is not null)
        {
            if ((await OnClose().ConfigureAwait(false)).Cancelled)
            {
                return;
            }
        }

        await ToggleDelayShowAsync().ConfigureAwait(false);
        await CurrentModel.CloseAsync(result ?? ModalResult.Cancel()).ConfigureAwait(false);
    }

    protected override async Task OnInitializedAsync()
    {
        await ToggleDelayShowAsync().ConfigureAwait(false);
    }

    private async Task ToggleDelayShowAsync() // need for animation showing
    {
        await Task.Delay(1).ConfigureAwait(false);
        delayedShow = string.IsNullOrEmpty(delayedShow) ? "show" : "";
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

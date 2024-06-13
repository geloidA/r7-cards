using System.Globalization;
using KolBlazor;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.Common;

public partial class Smooth : KolComponentBase
{
    private int _opacity;
    private string DelayString => Delay.TotalSeconds.ToString(CultureInfo.InvariantCulture);

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _opacity = 1;
            await Task.Delay(1);
            StateHasChanged();
        }
    }

    [Parameter]
    public TimeSpan Delay { get; set; } = TimeSpan.FromMilliseconds(250);

    private bool _isShowing;

    [Parameter]
    #pragma warning disable BL0007
    public bool IsShowing
    #pragma warning restore BL0007
    {
        get => _isShowing;
        set 
        {
            if (value != (_opacity == 1))
            {
                _isShowing = value;
                _opacity = _isShowing ? 1 : 0;
                StateHasChanged();
            }
        }
    }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
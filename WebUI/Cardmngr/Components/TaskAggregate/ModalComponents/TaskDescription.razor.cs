using KolBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Cardmngr.Components.TaskAggregate.ModalComponents;

public partial class TaskDescription : KolComponentBase
{
    private bool _showMore;
    private bool _hasMoreText;
    private ElementReference _elementRef;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = null!;

    [Parameter]
    public string? Description { get; set; }

    private string? CssStyleResult => $"max-height: {(_showMore ? "9999" : "100")}px; {Style}";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await Task.Delay(100); // wait for render
            await TriggerHeightMeasure();
        }
    }

    private void ToggleShowMore()
    {
        _showMore = !_showMore;
    }

    internal async Task TriggerHeightMeasure()
    {
        var elementHeight = await JSRuntime.InvokeAsync<int>("measureHeight", _elementRef);
        _hasMoreText = elementHeight == 100;        
        StateHasChanged();
    }
}

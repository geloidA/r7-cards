using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.Common;

public partial class DateView : ComponentBase
{
    private bool _popoverOpen;
    private string _popoverGuid = null!;

    [Parameter]
    public bool NotShowYearIfCurrent { get; set; }

    [Parameter]
    public bool ReadOnly { get; set; }

    [Parameter]
    public DateTime? Date { get; set; }

    [Parameter]
    public EventCallback<DateTime?> DateChanged { get; set; }

    [Parameter]
    public RenderFragment<DateTime?>? DateRender { get; set; }


    private async Task OnDateChanged(DateTime? newValue)
    {
        await DateChanged.InvokeAsync(newValue);
        await InvokeAsync(() => _popoverOpen = false);
    }

    private void OpenPopover()
    {
        if (!ReadOnly)
        {
            _popoverOpen = true;
        }
    }
}

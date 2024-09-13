using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.TaskAggregate;

public partial class DateRangeView : ComponentBase
{
    [Parameter]
    public DateTime? Start { get; set; }

    [Parameter]
    public DateTime? End { get; set; }

    [Parameter]
    public EventCallback<DateTime?> StartChanged { get; set; }

    [Parameter]
    public EventCallback<DateTime?> EndChanged { get; set; }

    [Parameter]
    public bool NotShowYearIfCurrent { get; set; }

    [Parameter]
    public bool ReadOnly { get; set; }

    [Parameter]
    public RenderFragment<DateTime?>? StartDateRender { get; set; }

    [Parameter]
    public RenderFragment<DateTime?>? EndDateRender { get; set; }
}

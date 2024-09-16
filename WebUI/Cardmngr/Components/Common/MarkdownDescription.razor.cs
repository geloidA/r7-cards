using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.Common;

public partial class MarkdownDescription : ComponentBase
{
    private bool _isEdit = false;

    [Parameter]
    public string Text { get; set; } = "";

    [Parameter]
    public EventCallback<string> TextChanged { get; set; }
}

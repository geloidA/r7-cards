using KolBlazor;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.Common;

public partial class Markdown : KolComponentBase
{
    private bool _isEdit = false;
    private string _text = "";

    [Parameter]
    public string? Text { get; set; }

    [Parameter]
    public EventCallback<string> TextChanged { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public string? Placeholder { get; set; }

    [Parameter]
    public string? DisabledPlaceholder { get; set; }

    [Parameter]
    public bool IsEdit { get; set; }

    [Parameter]
    public EventCallback<bool> IsEditChanged { get; set; }

    private async Task SubmitEditAsync()
    {
        await ChangeEditAsync(false);
        await TextChanged.InvokeAsync(string.IsNullOrWhiteSpace(_text) ? "" : _text);
    }

    private Task ChangeEditAsync(bool isEdit)
    {
        _isEdit = isEdit;
        return IsEditChanged.InvokeAsync(isEdit);
    }
}
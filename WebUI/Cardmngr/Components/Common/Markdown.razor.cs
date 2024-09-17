using KolBlazor;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.Common;

public partial class Markdown : KolComponentBase
{
    private bool _isEdit = false;
    private string _text = "";

    [Parameter]
    public string Text { get; set; } = "";

    [Parameter]
    public EventCallback<string> TextChanged { get; set; }

    private Task SubmitEditAsync()
    {
        _isEdit = false;
        return TextChanged.InvokeAsync(string.IsNullOrWhiteSpace(_text) ? "" : _text);
    }
}
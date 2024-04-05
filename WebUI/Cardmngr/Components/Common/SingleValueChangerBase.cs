using KolBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Cardmngr.Components.Common;

public abstract class SingleValueChangerBase<T> : KolComponentBase
{
    protected FluentInputBase<T>? Input;
    protected bool EditMode;

    [Parameter] public bool Disabled { get; set; }

    internal async Task ToggleEditMode()
    {
        if (PreventToggling()) return;

        EditMode = !EditMode;

        if (EditMode)
        {
            await Task.Delay(5); // wait input initialization
            Input?.FocusAsync();
        }
    }

    protected virtual bool PreventToggling() => false;
}

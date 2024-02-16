using Cardmngr.Shared.Extensions;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.Modals.Base;

public abstract class AddEditModalBase<TValue> : ComponentBase
{    
    protected TValue? buffer;

    [Parameter] public TValue Model { get; set; } = default!;
    [Parameter] public bool IsAdd { get; set; }

    protected override void OnInitialized()
    {
        buffer = IsAdd ? Model : Model.CloneJson();
    }

    public virtual string SubmitText => IsAdd ? "Создать" : "Сохранить";
}

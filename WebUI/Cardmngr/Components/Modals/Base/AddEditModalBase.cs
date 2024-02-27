using AutoMapper;
using Blazored.Modal;
using Blazored.Modal.Services;
using Cardmngr.Components.Modals.ConfirmModals;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.Modals.Base;

public abstract class AddEditModalBase<TModel, TUpdateData> : ComponentBase
    where TUpdateData : new()
{    
    protected TUpdateData buffer = new();

    [CascadingParameter(Name = "MiddleModal")] protected ModalOptions MiddleModal { get; set; } = null!;
    [CascadingParameter] protected IModalService Modal { get; set; } = null!;

    [Parameter] public TModel? Model { get; set; }
    [Parameter] public bool IsAdd { get; set; }

    [Inject] protected IMapper Mapper { get; set; } = null!;

    protected override void OnInitialized()
    {
        if (!IsAdd)
        {
            buffer = Mapper.Map<TUpdateData>(Model);
        }
    }

    public virtual string SubmitText => IsAdd ? "Создать" : "Сохранить";

    protected Task<ModalResult> ShowDeleteConfirm(string title, string message = "Вы уверены? Действие необратимо.")
    {
        return Modal.Show<DefaultConfirmModal>(
            title, 
            new ModalParameters { { "AdditionalText", message } }, 
            MiddleModal).Result;
    }
}

using AutoMapper;
using Blazored.Modal;
using Blazored.Modal.Services;
using Cardmngr.Components.Modals.ConfirmModals;
using Cardmngr.Shared.Utils.Comparer;
using Cardmngr.Utils;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.Modals.Base;

public abstract class AddEditModalBase<TModel, TUpdateData>(IEqualityComparer<TModel, TUpdateData> comparer) : ComponentBase
    where TUpdateData : new()
{
    private readonly IEqualityComparer<TModel, TUpdateData> comparer = comparer;
    protected TUpdateData Buffer = new();

    [CascadingParameter(Name = "MiddleModal")] protected ModalOptions MiddleModal { get; set; } = null!;
    [CascadingParameter] protected IModalService Modal { get; set; } = null!;

    [Parameter] public TModel? Model { get; set; }
    [Parameter] public bool IsAdd { get; set; }

    [Inject] protected IMapper Mapper { get; set; } = null!;

    protected override void OnInitialized()
    {
        if (!IsAdd)
        {
            Buffer = Mapper.Map<TUpdateData>(Model);
        }
    }

    protected virtual string SubmitText => IsAdd ? "Создать" : "Сохранить";

    /// <summary>
    /// Use to skip <see cref="ShowCloseConfirm"/> before closing
    /// </summary>
    protected bool SkipConfirmation { get; set; }

    protected virtual bool CanBeSaved { get; } = true;

    protected Task<ModalResult> ShowCloseConfirm()
    {
        if (SkipConfirmation)
        {
            return Task.FromResult(ModalResult.Ok());
        }

        if (!comparer.Equals(Model, Buffer) || !CanBeSaved)
        {
            return Modal.Show<DefaultConfirmModal>( 
                new ModalParameters 
                { 
                    { "AdditionalText", "Вы уверены, что хотите закрыть без сохранения?" },
                    { "HeaderText", "Закрыть без сохранения?" },
                    { "OkTitle", "Закрыть" },
                    { "OkBgColor", CardmngrColors.Accent },
                    { "OkTextColor", CardmngrColors.ForegroundOnAccent }
                }, 
                MiddleModal).Result;
        }

        return Task.FromResult(ModalResult.Ok());
    }

    protected Task<ModalResult>ShowDeleteConfirm(string title, string message = "Вы уверены? Действие необратимо.")
    {
        return Modal.Show<DefaultConfirmModal>(
            new ModalParameters 
            {
                { "OkTitle", "Удалить" },
                { "HeaderText", title },
                { "OkBgColor", CardmngrColors.Error },
                { "OkTextColor", "white" },
                { "AdditionalText", message } 
            }, 
            MiddleModal).Result;
    }
}

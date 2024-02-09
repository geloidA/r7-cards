using Blazored.Modal;
using Blazored.Modal.Services;
using Cardmngr.Components.Modals.ConfirmModals;
using Cardmngr.Models.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.Modals.DetailModals;

public abstract class ModelFormComponent<TModel, TApiModel> : ValidationComponent
    where TModel : class, IEditableModel<TApiModel>
{
    protected bool submiting;
    protected TModel changingState = null!;

    [CascadingParameter(Name = "MiddleModal")] protected ModalOptions ModalOptions { get; set; } = null!;
    [CascadingParameter] IModalService Modal { get; set; } = null!;

    [Parameter, EditorRequired] public TModel Model { get; set; } = null!;
    [Parameter] public bool IsCreation { get; set; }

    protected override void OnInitialized()
    {
        changingState = (TModel)Model.EditableModel;
    }

    protected async Task Submit()
    {
        submiting = true;

        var source = IsCreation ? await OnCreateAsync() : await OnUpdateAsync();

        if (source is not { })
        {
            throw new NullReferenceException("Source is null");
        }

        Model.Update(source);

        if (IsCreation) HookCreateSubmit(Model);
        else HookUpdateSubmit(Model);
        
        await CloseAsync();
    }

    public string LoadingText => IsCreation ? "Создание..." : "Сохранение...";
    public string SubmitText => IsCreation ? "Создать" : "Сохранить";

    private const string Empty = "";

    protected async Task DeleteAsync(bool confirmation, string confirmationText = Empty)
    {
        if (!confirmation)
        {
            await OnDeleteAsync();
            return;
        }

        var parameters = new ModalParameters
        {
            { "AdditionalText", confirmationText }
        };
        var result = await Modal.Show<DefaultConfirmModal>("Удаление", parameters, ModalOptions).Result;

        if (result.Confirmed)
        {
            await OnDeleteAsync();
            await CloseAsync();
        }
    }

    protected virtual void HookCreateSubmit(TModel updatedModel) { }
    protected virtual void HookUpdateSubmit(TModel updatedModel) { }

    protected abstract Task CloseAsync();
    protected abstract Task<TApiModel> OnCreateAsync();
    protected abstract Task<TApiModel> OnUpdateAsync();
    protected abstract Task OnDeleteAsync();
}

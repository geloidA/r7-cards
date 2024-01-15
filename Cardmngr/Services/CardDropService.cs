using BlazorCards;
using BlazorCards.Core;
using Blazored.Modal;
using Blazored.Modal.Services;
using Cardmngr.Components.Modals.ConfirmModals;
using Cardmngr.Extensions;
using Onlyoffice.Api.Common;
using Onlyoffice.Api.Logics;

namespace Cardmngr.Services;

public class CardDropService(IModalService modalService, IProjectApi projectApi)
{
    private readonly IModalService modalService = modalService;
    private readonly IProjectApi projectApi = projectApi;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="card"></param>
    /// <param name="to"></param>
    /// <returns>Returns true if card was dropped</returns>
    public async Task<bool> Drop(Card card, Card to)
    {
        var task = to.GetTask();
        return await UpdateWithCheck(card, (Status)task.Status, task.CustomTaskStatus);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="card"></param>
    /// <param name="to"></param>
    /// <returns>Returns true if card was dropped</returns>
    public async Task<bool> Drop(Card card, BoardColumn to)
    {
        var status = to.GetTaskStatus();
        return await UpdateWithCheck(card, (Status)status.StatusType, status.IsDefault ? null : status.Id);        
    }

    private async Task<bool> UpdateWithCheck(Card card, Status status, int? customStatus)
    {
        if (status == Status.Closed && !card.CanMarkClosed())
        {
            var modalRes = await ShowCloseCardConfirmModal();
            if (modalRes.Cancelled) return false;
            card.CloseSubtasks();
        }
        await UpdateCardStatus(card.GetTask(), status, customStatus);
        card.Column!.Remove(card);
        return true;
    }

    private async Task UpdateCardStatus(Onlyoffice.Api.Models.Task task, Status status, int? customStatus = null)
    {
        await projectApi.UpdateTaskStatusWithCheckAsync(task, status, customStatus);
        task.Status = (int)status;
        task.CustomTaskStatus = customStatus;
    }

    private async Task<ModalResult> ShowCloseCardConfirmModal()
    {
        var options = new ModalOptions { Position = ModalPosition.Middle, Size = ModalSize.Automatic };
        var modal = modalService.Show<CloseCardConfirmModal>("Закрытие задачи", options);
        return await modal.Result;
    }
}
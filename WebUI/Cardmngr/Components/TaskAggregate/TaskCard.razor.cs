using Blazored.Modal;
using Blazored.Modal.Services;
using Cardmngr.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Cardmngr.Shared.Extensions;
using Cardmngr.Components.ProjectAggregate;
using Cardmngr.Components.TaskAggregate.Modals;
using Cardmngr.Application.Clients.SignalRHubClients;
using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Services;

namespace Cardmngr.Components.TaskAggregate;

public partial class TaskCard : ComponentBase, IDisposable
{
    private List<TaskTag> taskTags = [];

    [Inject] ITaskClient TaskClient { get; set; } = null!;
    [Inject] TagColorGetter TagColorGetter { get; set; } = null!;

    [CascadingParameter] IProjectState State { get; set; } = null!;
    [CascadingParameter] ProjectHubClient? ProjectHubClient { get; set; }
    [CascadingParameter(Name = "DetailsModal")] ModalOptions DetailsModal { get; set; } = null!;
    [CascadingParameter] IModalService Modal { get; set; } = null!;

    [Parameter] public OnlyofficeTask Task { get; set; } = null!;

    string CssDeadline => Task.IsDeadlineOut() ? "red-border" : "";
    string CssMarginTitle => string.IsNullOrEmpty(Task.Description) ? "mb-5" : "";

    protected override async Task OnInitializedAsync()
    {        
        taskTags = await TaskClient.GetTaskTagsAsync(Task.Id).ToListAsync();
        if (State is IRefresheableProjectState refresheableProjectState)
        {
            refresheableProjectState.RefreshService.Refreshed += RefreshTaskTags;
        }
    }

    private async void RefreshTaskTags()
    {
        taskTags = await TaskClient.GetTaskTagsAsync(Task.Id).ToListAsync();
        InvokeAsync(StateHasChanged).Forget();
    }

    private async Task OpenModal()
    {
        var parameters = new ModalParameters
        {
            { "Model", Task },
            { "State", State },
            { "ProjectHubClient", ProjectHubClient },
            { "TaskTags", taskTags }
        };

        await Modal.Show<TaskDetailsModal>("", parameters, DetailsModal).Result;
    }

    private string GetDeadlineString()
    {
        if (Task.Deadline is null) return "Срок неизвестен";
        var diff = DateTime.Now.Date - Task.Deadline.Value.Date;
        return diff.TotalDays == 0
            ? "Срок истек - Сегодня"
            : $"Срок истек - {diff.TotalDays} {Utils.Common.GetDayNameByDayCount(diff.TotalDays)} назад";
    }

    public void Dispose()
    {
        if (State is IRefresheableProjectState refresheableProjectState)
        {
            refresheableProjectState.RefreshService.Refreshed -= RefreshTaskTags;
        }
    }
}

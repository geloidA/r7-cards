using Blazored.Modal;
using Blazored.Modal.Services;
using Cardmngr.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Cardmngr.Shared.Extensions;
using Cardmngr.Components.ProjectAggregate;
using Cardmngr.Components.TaskAggregate.Modals;
using Cardmngr.Application.Clients.SignalRHubClients;
using Cardmngr.Services;

namespace Cardmngr.Components.TaskAggregate;

public partial class TaskCard : ComponentBase
{
    [Inject] ITagColorManager TagColorGetter { get; set; } = null!;

    [CascadingParameter] IProjectState State { get; set; } = null!;
    [CascadingParameter] ProjectHubClient? ProjectHubClient { get; set; }
    [CascadingParameter(Name = "DetailsModal")] ModalOptions DetailsModal { get; set; } = null!;
    [CascadingParameter] IModalService Modal { get; set; } = null!;

    [Parameter] public OnlyofficeTask Task { get; set; } = null!;

    string CssMarginTitle => string.IsNullOrEmpty(Task.Description) ? "mb-5" : "";

    private string CssDeadline =>
        Task.Deadline is null ? "" :
        Task.IsDeadlineOut() ? "red-border" :
        Task.IsSevenDaysDeadlineOut() ? "warning-border" : "";

    protected override void OnInitialized()
    {
        openModalAction = OpenModal;
    }
    
    private int _opacity;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // opacity animation
            _opacity = 1;
            await System.Threading.Tasks.Task.Delay(1);
            StateHasChanged();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private Func<Task> openModalAction = null!; // TODO: maybe remove, because not improve performance too much
    private async Task OpenModal()
    {
        var parameters = new ModalParameters
        {
            { "Model", Task },
            { "State", State },
            { "ProjectHubClient", ProjectHubClient },
            { "TaskTags", Task.Tags }
        };

        await Modal.Show<TaskDetailsModal>("", parameters, DetailsModal).Result;
    }

    /// <summary>
    /// {0} - days left
    /// {1} - day count name
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    private string GetDeadlineString(string format)
    {
        if (Task.Deadline is null) return "Срок неизвестен";
        var diff = DateTime.Now.Date - Task.Deadline.Value.Date;
        return diff.TotalDays != 0
            ? string.Format(format, Math.Abs(diff.TotalDays), Utils.Common.GetDayNameByDayCount(diff.TotalDays))
            : "Крайний срок истечет сегодня";
    }
}

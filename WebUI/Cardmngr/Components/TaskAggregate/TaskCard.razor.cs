using Blazored.Modal;
using Blazored.Modal.Services;
using Cardmngr.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Onlyoffice.Api.Logics;
using Cardmngr.Shared.Extensions;
using Cardmngr.Components.ProjectAggregate;
using Cardmngr.Components.Modals.DetailModals;

namespace Cardmngr.Components.TaskAggregate;

public partial class TaskCard
{
    [CascadingParameter] ProjectState State { get; set; } = null!;
    [CascadingParameter(Name = "DetailsModal")] ModalOptions DetailsModal { get; set; } = null!;
    [CascadingParameter] IModalService Modal { get; set; } = null!;

    [CascadingParameter] IProjectApi ProjectApi { get; set; } = null!;
    [Parameter] public OnlyofficeTask Task { get; set; } = null!;
    
    string CssDeadline => Task.IsDeadlineOut() ? "red-border" : "";

    private async Task OpenModal()
    {
        var parameters = new ModalParameters
        {
            { "Model", Task },
            { "ProjectApi", ProjectApi }
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
}

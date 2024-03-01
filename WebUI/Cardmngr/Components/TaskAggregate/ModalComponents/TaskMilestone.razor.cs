using Blazored.Modal;
using Blazored.Modal.Services;
using Cardmngr.Components.ProjectAggregate;
using Cardmngr.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Onlyoffice.Api.Models;
using Cardmngr.Components.Modals;

namespace Cardmngr.Components.TaskAggregate.ModalComponents;

public partial class TaskMilestone
{
    [CascadingParameter] IProjectState State { get; set; } = null!;
    [CascadingParameter(Name = "MiddleModal")] ModalOptions ModalOptions { get; set; } = null!;
    [CascadingParameter] IModalService Modal { get; set; } = null!;

    [Parameter] public TaskUpdateData Task { get; set; } = null!;
    [Parameter] public bool Disabled { get; set; }

    private async Task ShowSelectionModalAsync()
    {
        var parameters = new ModalParameters
        {
            { "Items", State.Model!.Milestones },
            { "ItemRender", RenderMilestone }
        };

        var res = await Modal.Show<SelectionModal<Milestone>>("Выберите веху", parameters, ModalOptions).Result;

        if (res.Confirmed)
        {
            var milestone = (Milestone)res.Data!;
            Task.MilestoneId = milestone.Id;
        }
    }

    private void DeleteMilestone()
    {
        Task.MilestoneId = null;
    }
}

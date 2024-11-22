﻿using Blazored.Modal;
using Blazored.Modal.Services;
using Cardmngr.Components.ProjectAggregate.States;
using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Extensions;
using Microsoft.AspNetCore.Components;
using Cardmngr.Components.TaskAggregate.Modals;

namespace Cardmngr.Components.TaskStatusAggregate;

public partial class TaskStatusHeader
{
    [CascadingParameter] IProjectState State { get; set; } = null!;

    [CascadingParameter(Name = "DetailsModal")] ModalOptions DetailsOptions { get; set; } = null!;
    [CascadingParameter] IModalService Modal { get; set; } = default!;

    [Parameter] public OnlyofficeTaskStatus TaskStatus { get; set; } = null!;

    [Parameter] public bool IsCollapsed { get; set; }

    public int TaskCount => State.Tasks.FilterByStatus(TaskStatus).Count();

    private async Task ShowCreateTaskModal()
    {
        var parameters = new ModalParameters
        {
            { "State", State },
            { "IsAdd", true },
            { "TaskStatusId", TaskStatus.Id }
        };

        await Modal.Show<TaskDetailsModal>(parameters, DetailsOptions).Result;
    }
}

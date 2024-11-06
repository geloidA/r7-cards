using Blazored.Modal;
using Blazored.Modal.Services;
using Cardmngr.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Cardmngr.Components.ProjectAggregate.States;
using Cardmngr.Components.TaskAggregate.Modals;
using Cardmngr.Components.ProjectAggregate.Models;
using Cardmngr.Services;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components;
using Cardmngr.Components.TaskAggregate.ModalComponents;

namespace Cardmngr.Components.TaskAggregate;

public sealed partial class TaskCard : ComponentBase, IDisposable
{
    private TaskDescription? _taskDescription;

    [Inject] private ITagColorManager TagColorGetter { get; set; } = null!;

    [CascadingParameter] private TimeAgoOptions TimeAgoOpt { get; set; } = null!;
    [CascadingParameter] private IProjectState State { get; set; } = null!;

    [CascadingParameter(Name = "DetailsModal")]
    private ModalOptions DetailsModal { get; set; } = null!;

    [CascadingParameter] private IModalService Modal { get; set; } = null!;

    [Parameter] public OnlyofficeTask Task { get; set; } = null!;

    private string CssMarginTitle => string.IsNullOrEmpty(Task.Description) ? "mb-5" : "";

    protected override void OnInitialized()
    {
        State.MilestonesChanged += OnMilestoneChanged;
    }

    private async Task OpenModal()
    {
        var parameters = new ModalParameters
        {
            { "Model", Task },
            { "State", State }
        };

        var res = await Modal.Show<TaskDetailsModal>(parameters, DetailsModal).Result;

        if (res.Confirmed && _taskDescription != null)
        {
            await _taskDescription.TriggerHeightMeasure();
        }

        Console.WriteLine(Task.Tags.Count);
    }

    private void OnMilestoneChanged(EntityChangedEventArgs<Milestone>? args)
    {
        if (args is { ActionType: not EntityActionType.Add or not EntityActionType.None, Entity.Id: var id } && id == Task.MilestoneId)
        {
            StateHasChanged();
        }
    }

    private string GetDeadlineString()
    {
        if (Task.Deadline is null) return "Срок неизвестен";
        var diff = DateTime.Now - Task.Deadline.Value;
        return $"Истек {diff.ToTimeAgo(TimeAgoOpt)}";
    }

    public void Dispose()
    {
        State.MilestonesChanged -= OnMilestoneChanged;
    }
}

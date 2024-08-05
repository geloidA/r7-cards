using Blazored.Modal;
using Blazored.Modal.Services;
using Cardmngr.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Cardmngr.Components.ProjectAggregate.States;
using Cardmngr.Components.TaskAggregate.Modals;
using Cardmngr.Components.ProjectAggregate.Models;
using Cardmngr.Services;

namespace Cardmngr.Components.TaskAggregate;

public sealed partial class TaskCard : ComponentBase, IDisposable
{
    [Inject] private ITagColorManager TagColorGetter { get; set; } = null!;

    [CascadingParameter] private IProjectState State { get; set; } = null!;

    [CascadingParameter(Name = "DetailsModal")]
    private ModalOptions DetailsModal { get; set; } = null!;

    [CascadingParameter] private IModalService Modal { get; set; } = null!;

    [Parameter] public OnlyofficeTask Task { get; set; } = null!;

    private string CssMarginTitle => string.IsNullOrEmpty(Task.Description) ? "mb-5" : "";

    protected override void OnInitialized()
    {
        openModalAction = OpenModal;

        State.MilestonesChanged += OnMilestoneChanged;
    }

    private Func<Task> openModalAction = null!; // TODO: maybe remove, because not improve performance too much

    private async Task OpenModal()
    {
        var parameters = new ModalParameters
        {
            { "Model", Task },
            { "State", State },
            { "TaskTags", Task.Tags }
        };

        await Modal.Show<TaskDetailsModal>("", parameters, DetailsModal).Result;
    }

    private void OnMilestoneChanged(EntityChangedEventArgs<Milestone>? args)
    {
        if (args is { ActionType: EntityActionType.Update, Entity.Id: var id } && id == Task.MilestoneId)
        {
            StateHasChanged();
        }
    }

/// <summary>
    /// Returns a string that represents the deadline of the task. 
    /// If the deadline is not known, the string "Срок неизвестен" is returned.
    /// If the deadline is today, the string "Истечет сегодня" is returned.
    /// If the deadline is tomorrow, the string "Истечет завтра" is returned.
    /// If the deadline is in the future, the string "{days left} {day count name} осталось" is returned,
    /// where {days left} is the number of days left until the deadline and {day count name} 
    /// is the name of the day count (e.g. день, дня, дней).
    /// </summary>
    /// <param name="format">The format string to use for the returned string.</param>
    /// <returns>A string that represents the deadline of the task.</returns>
    private string GetDeadlineString(string format)
    {
        if (Task.Deadline is null) return "Срок неизвестен";
        var diff = DateTime.Now.Date - Task.Deadline.Value.Date;
        return diff.TotalDays != 0
            ? string.Format(format, Math.Abs(diff.TotalDays), Utils.Common.GetDayNameByDayCount(diff.TotalDays))
            : "Истечет сегодня";
    }

    public void Dispose()
    {
        State.MilestonesChanged -= OnMilestoneChanged;
    }
}

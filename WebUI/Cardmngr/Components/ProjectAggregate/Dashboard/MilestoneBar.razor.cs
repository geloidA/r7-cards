using System.Globalization;
using Cardmngr.Components.ProjectAggregate.States;
using Cardmngr.Domain.Entities;
using Cardmngr.Extensions;
using Cardmngr.Shared.Extensions;
using KolBlazor;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.ProjectAggregate.Dashboard;

public partial class MilestoneBar : KolComponentBase
{
    [CascadingParameter] IProjectState State { get; set; } = null!;

    [Parameter, EditorRequired] public Milestone Milestone { get; set; } = null!;

    protected override void OnInitialized()
    {
        State.TasksChanged += _ => StateHasChanged();
    }

    private string CssBackgroundColor => Milestone.IsDeadlineOut() ? "background: var(--neutral-fill-strong-rest);" : "background: #6dc07b;";

    private string CompleteProgress
    {
        get
        {
            var closed = State.GetMilestoneTasks(Milestone).Count(x => x.IsClosed());
            if (Milestone.IsClosed() || (Milestone.IsDeadlineOut() && closed == 0)) return "width: 100%;";
            if (!State.GetMilestoneTasks(Milestone).Any()) return "width: 0%;";

            var completeProcentage = closed * 100.0 / State.GetMilestoneTasks(Milestone).Count();
            return $"width: {completeProcentage.ToString(CultureInfo.InvariantCulture)}%;";
        }
    }
}

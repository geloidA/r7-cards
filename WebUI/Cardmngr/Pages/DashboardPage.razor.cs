using Cardmngr.Application;
using Cardmngr.Components.ProjectAggregate.States;
using Cardmngr.Domain.Entities;
using Cardmngr.Domain.Enums;
using Cardmngr.Extensions;
using Cardmngr.Shared.Extensions;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Pages;

public partial class DashboardPage : ComponentBase
{
    private (string Header, Func<IFilterableProjectState, IList<OnlyofficeTask>> TaskRefreshFunc)[] _columns = null!;

    [Inject] ITaskStatusClient TaskStatusClient { get; set; } = null!;

    [Parameter] public int ProjectId { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var defaultOpenStatus = await TaskStatusClient
            .GetAllAsync()
            .SingleAsync(s => s.StatusType == StatusType.Open && s.IsDefault);
        
        _columns = InitializeColumns(defaultOpenStatus);
    }

    private static (string Header, Func<IFilterableProjectState, IList<OnlyofficeTask>> TaskRefreshFunc)[] InitializeColumns(
        OnlyofficeTaskStatus defaultOpenStatus)
    {
        return 
        [
            ("Просрочено", (state) => [.. state.FilteredTasks()
                .Where(t => t.IsDeadlineOut())
                .OrderByTaskCriteria()]),

            ("Скоро подойдет к концу", (state) => [.. state.FilteredTasks()
                .Where(t => t.IsSevenDaysDeadlineOut() && !t.IsDeadlineOut())
                .OrderByTaskCriteria()]),

            ("Предстоит выполнить", (state) => [.. state.FilteredTasks()
                .Where(t => !t.IsSevenDaysDeadlineOut() && 
                    !t.IsDeadlineOut() && 
                    t.TaskStatusId == defaultOpenStatus.Id)
                .OrderByTaskCriteria()]),
        ];
    }
}

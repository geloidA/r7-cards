using Cardmngr.Domain;
using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Extensions;

namespace Cardmngr.Reports.Base;

public class EfficiencyFactorReportData
{
    private readonly IEnumerable<OnlyofficeTask> onlyofficeTasks;

    private EfficiencyFactorReportData() : this([]) { }

    private EfficiencyFactorReportData(IEnumerable<OnlyofficeTask> tasks)
    {
        onlyofficeTasks = tasks;
    }

    public static EfficiencyFactorReportData Create(IEnumerable<OnlyofficeTask> tasks) => new(tasks);

    public IEnumerable<IGrouping<UserInfo, IGrouping<ProjectInfo, OnlyofficeTask>>> GroupByUser
    {
        get => onlyofficeTasks
            .SelectMany(x => x.Responsibles, (task, user) => new { task, user })
            .GroupBy(x => x.user, x => x.task)
            .Select(tasks => new Grouping<UserInfo, IGrouping<ProjectInfo, OnlyofficeTask>>(tasks.Key, tasks
                .OrderBy(t => t.IsDeadlineOut())
                .ThenByDescending(x => x.Status)
                .ThenBy(t => t.Deadline)
                .GroupBy(y => y.ProjectOwner)));
    }
}

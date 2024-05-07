using System.Diagnostics.CodeAnalysis;
using Cardmngr.Domain;
using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Extensions;

namespace Cardmngr.Reports.Base;

public class EfficiencyFactorReportData
{
    private readonly IEnumerable<OnlyofficeTask> onlyofficeTasks;
    private readonly IEnumerable<UserInfo> users;

    private EfficiencyFactorReportData() : this([], []) { }

    private EfficiencyFactorReportData(IEnumerable<OnlyofficeTask> tasks, IEnumerable<UserInfo> users)
    {
        onlyofficeTasks = tasks;
        this.users = users;
    }

    public static EfficiencyFactorReportData Create(IEnumerable<OnlyofficeTask> tasks, IEnumerable<UserInfo> users) => new(tasks, users);

    public IEnumerable<IGrouping<UserInfo, IGrouping<ProjectInfo, OnlyofficeTask>>> GroupByUser
    {
        get => onlyofficeTasks
            .SelectMany(x => GetSelectedUsers(x.Responsibles), (task, user) => new { task, user })
            .GroupBy(x => x.user, x => x.task)
            .Select(tasks => new Grouping<UserInfo, IGrouping<ProjectInfo, OnlyofficeTask>>(tasks.Key, tasks
                .OrderBy(t => t.IsDeadlineOut())
                .ThenByDescending(x => x.Status)
                .ThenBy(t => t.Deadline)
                .GroupBy(y => y.ProjectOwner)));
    }

    private IEnumerable<UserInfo> GetSelectedUsers(IEnumerable<UserInfo> responsibles) => users.Any()
        ? users.Intersect(responsibles, UserInfoEqualityComparer.Instance)
        : responsibles; 
}

internal class UserInfoEqualityComparer : IEqualityComparer<UserInfo>
{
    public static readonly UserInfoEqualityComparer Instance = new();

    public bool Equals(UserInfo? x, UserInfo? y)
    {
        if (x == null || y == null) return false;
        return x.Id == y.Id;
    }

    public int GetHashCode([DisallowNull] UserInfo obj)
    {
        return obj.Id.GetHashCode();
    }
}

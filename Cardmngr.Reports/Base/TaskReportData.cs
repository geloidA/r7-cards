using Cardmngr.Domain;
using Cardmngr.Domain.Entities;

using GroupedTask = System.Collections.Generic.IEnumerable<System.Linq.IGrouping<Cardmngr.Domain.ProjectInfo, System.Linq.IGrouping<Cardmngr.Domain.MilestoneInfo, Cardmngr.Reports.IOnlyofficeTaskReportData>>>;
using TaskMapper = System.Func<Cardmngr.Domain.Entities.OnlyofficeTask, Cardmngr.Reports.IOnlyofficeTaskReportData>;

namespace Cardmngr.Reports.Base;

public class TaskReportData
{
    private readonly IEnumerable<OnlyofficeTask> tasks = [];
    private readonly TaskMapper mapper;

    private TaskReportData(TaskMapper mapper) => this.mapper = mapper;
    private TaskReportData(IEnumerable<OnlyofficeTask> tasks, TaskMapper mapper) : this(mapper) => this.tasks = tasks;

    public static TaskReportData Create(IEnumerable<OnlyofficeTask> tasks, TaskMapper mapper) => new(tasks, mapper);

    public GroupedTask GroupedTasks
    {
        get
        {
            return tasks
                .GroupBy(x => x.ProjectOwner)
                .Select(projTasks => new Grouping<ProjectInfo, IGrouping<MilestoneInfo, IOnlyofficeTaskReportData>>(
                        projTasks.Key, 
                        projTasks
                            .GroupBy(x => x.Milestone)
                            .Select(x => new Grouping<MilestoneInfo, IOnlyofficeTaskReportData>(x.Key, x.Select(mapper)))));
        }
    }
}

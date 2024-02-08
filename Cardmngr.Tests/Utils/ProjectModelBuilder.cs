using Onlyoffice.Api.Models;

using MyTask = Onlyoffice.Api.Models.Task;
using MyTaskStatus = Onlyoffice.Api.Models.TaskStatus;

namespace Cardmngr.Tests.Utils;

public class ProjectModelBuilder
{
    private readonly Random random = new();
    private Project project = new() { Responsible = new Responsible { Id = "1" }, CreatedBy = new CreatedBy { Id = "1" } };
    private List<MyTask> tasks = [new MyTask { Id = 1 }, new MyTask { Id = 2, MilestoneId = 1 }];
    private List<MyTaskStatus> statuses = [new MyTaskStatus { Id = 1 }, new MyTaskStatus { Id = 2 }];
    private List<Milestone> milestones = [new Milestone { Id = 1, Deadline = DateTime.Now }, new Milestone { Id = 2, Deadline = DateTime.Now }];
    private List<IUser> team = [new User("1"), new User("2")];

    public ProjectModelBuilder WithTeam(List<IUser> team)
    {
        ArgumentExceptionWrapper.ThrowIfNullOrEmpty(team);

        this.team = team;
        return this;
    }

    public ProjectModelBuilder WithCreatedBy(CreatedBy user)
    {
        project.CreatedBy = user;
        return this;
    }

    public ProjectModelBuilder WithResponsible(Responsible user)
    {
        project.Responsible = user;
        return this;
    }

    public ProjectModelBuilder WithTask(MyTask task)
    {
        tasks.Add(RandomizeValues(task));
        return this;
    }

    private MyTask RandomizeValues(MyTask task)
    {
        task.MilestoneId = GetRandomId<int?, Milestone>(milestones);
        task.CustomTaskStatus = GetRandomId<int?, MyTaskStatus>(statuses);
        return task;
    }

    private TResult? GetRandomId<TResult, TValue>(IList<TValue> values)
    {
        var index = random.Next(-1, values.Count);
        if (index == -1) return default;
        
        var idProperty = typeof(TValue).GetProperty("Id");

        return (TResult?)idProperty?.GetValue(values[index]);
    }

    public ProjectModelBuilder WithStatus(MyTaskStatus status)
    {
        statuses.Add(status);
        return this;
    }

    public ProjectModelBuilder WithMilestone(Milestone milestone)
    {
        milestones.Add(milestone);
        return this;
    }

    public ProjectModelBuilder WithTeamMember(IUser user)
    {
        team.Add(user);
        return this;
    }

    public ProjectModelBuilder WithProject(Project project)
    {
        this.project = project;
        return this;
    }

    public ProjectModelBuilder WithMilestones(List<Milestone> milestones)
    {
        ArgumentExceptionWrapper.ThrowIfNullOrEmpty(milestones);

        this.milestones = milestones;
        return this;
    }

    public ProjectModelBuilder WithTasks(List<MyTask> tasks)
    {
        ArgumentExceptionWrapper.ThrowIfNullOrEmpty(tasks);

        this.tasks = tasks;

        foreach (var task in tasks)
        {
            RandomizeValues(task);
        }

        return this;
    }

    public ProjectModelBuilder WithStatuses(List<MyTaskStatus> statuses)
    {
        ArgumentExceptionWrapper.ThrowIfNullOrEmpty(statuses);

        this.statuses = statuses;
        return this;
    }

    public ProjectModel Build()
    {        
        return new ProjectModel(project, tasks, statuses, milestones, team);
    }
}

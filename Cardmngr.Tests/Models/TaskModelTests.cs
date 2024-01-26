using System.Reflection;
using Onlyoffice.Api.Models;
using Cardmngr.Models.Attributes;
using OTask = Onlyoffice.Api.Models.Task;
using Cardmngr.Models;

namespace Cardmngr.Tests.Models;

public class TaskModelTests
{
    [Fact]
    public void Update_NotThrowException_WhenCorrect()
    {
        var project = ModelCreator.GetModel();

        var source = new OTask { };

        project.Tasks.First().Update(source);
    }

    [Fact]
    public void Update_UpdatesCorrectly()
    {
        var project = ModelCreator.GetModel();

        var source = new OTask
        {
            CanEdit = true,
            CanCreateSubtask = true,
            CanCreateTimeSpend = false,
            CanDelete = true,
            CanReadFiles = false,
            Id = 3,
            Title = "Another Test Task",
            Description = "This is another test task for illustration",
            Priority = 3,
            MilestoneId = 2,
            Milestone = new Milestone(),
            ProjectOwner = new ProjectOwner(),
            Subtasks = [],
            Status = 2,
            Progress = 75,
            UpdatedBy = new UpdatedBy(),
            Created = DateTime.Now,
            CreatedBy = new CreatedBy(),
            Updated = DateTime.Now.AddDays(-2),
            Responsibles = [],
            CustomTaskStatus = 3,
            Deadline = DateTime.Now.AddDays(10),
            StartDate = DateTime.Now.AddDays(-5)
        };

        var task = project.Tasks.First();

        task.Update(source);

        var taskType = task.GetType();
        var sourceType = source.GetType();

        var updatableProperties = taskType.GetProperties().Where(p => p.GetCustomAttribute<UpdatableAttribute>() is { });

        foreach (var prop in updatableProperties)
        {
            var sourceProp = sourceType.GetProperty(prop.Name) ?? throw new InvalidOperationException();

            if (prop.PropertyType.IsEnum)
            {
                Assert.Equal((int)sourceProp.GetValue(source)!, (int)prop.GetValue(task)!);
            }
            else
            {
                Assert.Equal(sourceProp.GetValue(source), prop.GetValue(task));
            }
        }

        Assert.Equal(source.Subtasks.Count, task.Subtasks.Count);
        Assert.Equal(source.Responsibles.Count, task.Responsibles.Count);
    }

    [Fact]
    public void Update_NotUpdate_UnupdatableProperties()
    {
        var project = ModelCreator.GetModel();

        var source = new OTask
        {
            CanEdit = true,
            CanCreateSubtask = true,
            CanCreateTimeSpend = false,
            CanDelete = true,
            CanReadFiles = false,
            Id = 3,
            Title = "Another Test Task",
            Description = "This is another test task for illustration",
            Priority = 3,
            MilestoneId = 2,
            Milestone = new Milestone(),
            ProjectOwner = new ProjectOwner(),
            Subtasks = [],
            Status = 2,
            Progress = 75,
            UpdatedBy = new UpdatedBy(),
            Created = DateTime.Now,
            CreatedBy = new CreatedBy(),
            Updated = DateTime.Now.AddDays(-2),
            Responsibles = [],
            CustomTaskStatus = 3,
            Deadline = DateTime.Now.AddDays(10),
            StartDate = DateTime.Now.AddDays(-5)
        };

        var task = project.Tasks.First();
        var copy = task.Clone(true);

        task.Update(source);

        var taskType = task.GetType();

        var unupdatableProps = taskType.GetProperties().Where(p => p.GetCustomAttribute<UpdatableAttribute>() == null);

        foreach (var prop in unupdatableProps)
        {
            if (prop.Name == nameof(TaskModel.Subtasks) || prop.Name == nameof(TaskModel.Responsibles) || prop.Name == nameof(TaskModel.Milestone)) continue; // TODO: refactor
            Assert.Equal(prop.GetValue(copy), prop.GetValue(task));
        }
    }

    [Fact]
    public void Update_ThrowArgumentNullException()
    {
        var project = ModelCreator.GetModel();

        Assert.Throws<ArgumentNullException>(() => project.Tasks.First().Update(null!));
    }
}

using Onlyoffice.Api.Models;
using FluentAssertions;

using MyTask = Onlyoffice.Api.Models.Task;
using MyTaskStatus = Onlyoffice.Api.Models.TaskStatus;
using Cardmngr.Extensions;

namespace Cardmngr.Tests.Utils;

public class ProjectModelBuilderTests
{
    [Fact]
    public void WithTeam_ShouldSetTeamMembers()
    {
        // Arrange
        var builder = new ProjectModelBuilder();
        var team = new List<IUser> { new User("3"), new User("4") };

        // Act
        var result = builder.WithTeam(team);

        // Assert
        Assert.Equal(team, result.Build().Team);
    }

    [Fact]
    public void WithCreatedBy_ShouldSetCreatedByUser()
    {
        // Arrange
        var builder = new ProjectModelBuilder();
        var user = new CreatedBy { Id = "2" };

        // Act
        var result = builder.WithCreatedBy(user);

        // Assert
        result.Build().CreatedBy.Should().BeEquivalentTo(user);
    }

    [Fact]
    public void WithResponsible_ShouldSetResponsibleUser()
    {
        // Arrange
        var builder = new ProjectModelBuilder();
        var user = new Responsible { Id = "3" };

        // Act
        var result = builder.WithResponsible(user);

        // Assert
        result.Build().Responsible.Should().BeEquivalentTo(user);
    }

    [Fact]
    public void WithTask_ShouldAddTask()
    {
        // Arrange
        var builder = new ProjectModelBuilder();
        var task = new MyTask { Id = 3 };

        // Act
        var result = builder.WithTask(task);

        // Assert
        Assert.NotNull(result.Build().Tasks().Single(x => x.Id == 3));
    }

    [Fact]
    public void WithStatus_ShouldAddStatus()
    {
        // Arrange
        var builder = new ProjectModelBuilder();
        var status = new MyTaskStatus { Id = 3 };

        // Act
        var result = builder.WithStatus(status);

        // Assert
        Assert.NotNull(result.Build().StatusBoard.Single(x => x.Id == 3));
    }

    [Fact]
    public void WithMilestone_ShouldAddMilestone()
    {
        // Arrange
        var builder = new ProjectModelBuilder();
        var milestone = new Milestone { Id = 3, Deadline = DateTime.Now };

        // Act
        var result = builder.WithMilestone(milestone);

        // Assert
        Assert.NotNull(result.Build().Milestones.Single(x => x.Id == 3));
    }

    [Fact]
    public void WithTeamMember_ShouldAddTeamMember()
    {
        // Arrange
        var builder = new ProjectModelBuilder();
        var user = new User("3");

        // Act
        var result = builder.WithTeamMember(user);

        // Assert
        Assert.Contains(user, result.Build().Team);
    }

    [Fact]
    public void WithProject_ShouldSetProjectDetails()
    {
        // Arrange
        var builder = new ProjectModelBuilder();
        var project = new Project { CreatedBy = new CreatedBy { Id = "3" }, Responsible = new Responsible { Id = "4" } };

        // Act
        var result = builder.WithProject(project);

        // Assert
        Assert.Equal(project.CreatedBy.Id, result.Build().CreatedBy!.Id);
        Assert.Equal(project.Responsible.Id, result.Build().Responsible!.Id);
    }

    [Fact]
    public void WithMilestones_ShouldSetMilestones()
    {
        // Arrange
        var builder = new ProjectModelBuilder();
        var milestones = new List<Milestone> { new() { Id = 3, Deadline = DateTime.Now } };

        // Act
        var result = builder.WithMilestones(milestones);

        // Assert
        Assert.NotNull(result.Build().Milestones.Single(x => x.Id == 3));
    }

    [Fact]
    public void WithTasks_ShouldSetTasks()
    {
        // Arrange
        var builder = new ProjectModelBuilder();
        var tasks = new List<MyTask> { new() { Id = 3 } };

        // Act
        var result = builder.WithTasks(tasks);

        // Assert
        Assert.NotNull(result.Build().Tasks().Single(x => x.Id == 3));
    }

    [Fact]
    public void WithStatuses_ShouldSetStatuses()
    {
        // Arrange
        var builder = new ProjectModelBuilder();
        var statuses = new List<MyTaskStatus> { new() { Id = 3 } };

        // Act
        var result = builder.WithStatuses(statuses);

        // Assert
        var columns = result.Build().StatusBoard;
        Assert.NotNull(columns.Single(x => x.Id == 3));
    }

    [Fact]
    public void Build_ShouldCreateProjectModel()
    {
        // Arrange
        var builder = new ProjectModelBuilder();

        // Act
        var result = builder.Build();

        // Assert
        Assert.IsType<ProjectModel>(result);
    }

    [Fact]
    public void WithStatuses_WhenListIsEmpty_ThrowsArgumentException()
    {
        // Arrange
        var builder = new ProjectModelBuilder();
        var emptyList = new List<MyTaskStatus>();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => builder.WithStatuses(emptyList));
    }
}

using Cardmngr.Domain.Entities;
using Cardmngr.Shared.Extensions;
using FluentAssertions;

namespace Cardmngr.Shared.Tests.ExtensionTests.TaskExtensionsTests;

public class OrderTaskExtensionsTests
{
    [Fact]
    public void OrderByTaskCriteria_ShouldReturnEmpty_WhenIsEmpty()
    {
        var tasks = new List<OnlyofficeTask>();

        tasks.OrderByTaskCriteria().Should().BeEmpty();
    }

    [Fact]
    public void OrderByTaskCriteria_ShouldReturnCorrectOrder()
    {
        var priorityTask = new OnlyofficeTask 
        { 
            Title = "Priority task", 
            Status = Domain.Enums.Status.Open, 
            Deadline = DateTime.Now.AddYears(1), 
            Priority = Domain.Enums.Priority.High 
        };
        var firstTask = new OnlyofficeTask { Title = "First task", Status = Domain.Enums.Status.Open, Deadline = DateTime.Now.AddYears(1) };
        var secondTask = new OnlyofficeTask { Title = "Second task", Status = Domain.Enums.Status.Open, Deadline = DateTime.Now.AddYears(2) };
        var deadlineTask = new OnlyofficeTask { Title = "Deadline task", Status = Domain.Enums.Status.Open, Deadline = DateTime.Now.AddDays(-1) };
        var closedTask = new OnlyofficeTask { Title = "Closed task", Status = Domain.Enums.Status.Closed, Deadline = new DateTime(2022, 2, 1) };

        List<OnlyofficeTask> tasks = [secondTask, firstTask, priorityTask, deadlineTask, closedTask];

        tasks
            .OrderByTaskCriteria()
            .Should()
            .ContainInOrder([deadlineTask, priorityTask, firstTask, secondTask, closedTask]);
    }
}

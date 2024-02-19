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
        var priorityTask = new OnlyofficeTask { Priority = Domain.Enums.Priority.High };
        var firstTask = new OnlyofficeTask { StartDate = new DateTime(2022, 2, 1) };
        var secondTask = new OnlyofficeTask { StartDate = new DateTime(2022, 2, 5) };
        var deadlineTask = new OnlyofficeTask { Deadline = DateTime.Now.AddDays(-1) };
        var closedTask = new OnlyofficeTask { Status = Domain.Enums.Status.Closed, StartDate = new DateTime(2022, 2, 1) };

        List<OnlyofficeTask> tasks = [secondTask, firstTask, priorityTask, deadlineTask];

        tasks.OrderByTaskCriteria().Should().Equal(deadlineTask, priorityTask, firstTask, secondTask, closedTask);
    }
}

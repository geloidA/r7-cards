using BlazorCards;
using BlazorCards.Core;
using Onlyoffice.Api.Models.Extensions;
using Onlyoffice.Api.Models;
using Onlyoffice.Api.Common;

namespace Cardmngr.Extensions;

public static class BlazorCardExtensions
{
    public static Onlyoffice.Api.Models.Task GetTask(this Card card)
    {
        return card.Data as Onlyoffice.Api.Models.Task ?? 
            throw new InvalidOperationException("Card data is not a task");
    }

    public static Onlyoffice.Api.Models.TaskStatus GetTaskStatus(this BoardColumn column)
    {
        return column.Data as Onlyoffice.Api.Models.TaskStatus ?? 
            throw new InvalidOperationException("Column data is not a task status");
    }

    public static Project GetProject(this Board board)
    {
        return board.Data as Project ??
            throw new InvalidOperationException("Board data is not a project");
    }

    public static bool CanMarkClosed(this Card card)
    {
        return card.GetTask().CanMarkClosed();
    }

    public static void CloseSubtasks(this Card card)
    {
        var task = card.GetTask();

        if (task.Subtasks is not { } || task.Subtasks.Count == 0)
            throw new InvalidOperationException("No subtasks found");

        foreach (var subtask in task.Subtasks)
        {
            subtask.Status = (int)Status.Closed;
        }
    }
}

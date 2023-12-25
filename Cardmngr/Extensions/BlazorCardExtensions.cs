using BlazorCards;
using BlazorCards.Core;
using Onlyoffice.Api.Models;

namespace Cardmngr;

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
}

using BlazorCards.Core;
using Onlyoffice.Api.Models;

namespace Cardmngr.Utils;

public class ModelConverter
{
    public static Board ConvertToBoard(Project proj, List<Onlyoffice.Api.Models.Task> tasks, IEnumerable<Onlyoffice.Api.Models.TaskStatus> statuses)
    {
        return new Board(statuses, DivideOnStatues(statuses, tasks), proj.Title!, proj)
        {
            CssName = "board"
        };
    }

    private static IEnumerable<IEnumerable<Onlyoffice.Api.Models.Task>> DivideOnStatues(
        IEnumerable<Onlyoffice.Api.Models.TaskStatus> statuses,
        IEnumerable<Onlyoffice.Api.Models.Task> tasks)
    {
        return statuses
            .Select(x => tasks
                .Where(t => t.CustomTaskStatus.HasValue ? t.CustomTaskStatus.Value == x.Id : 
                    t.Status == x.StatusType && x.IsDefault)
                .OrderBy(x => (-x.Priority, x.Deadline ?? DateTime.MaxValue))
                .ThenByDescending(x => x.Updated));
    }
}

using Cardmngr.Domain.Entities;

namespace Cardmngr.Shared.Utils.Filter.TaskFilters;

public class StatusTaskFilter(string status, IEnumerable<OnlyofficeTaskStatus> statuses) : IFilter<OnlyofficeTask>
{
    public bool Filter(OnlyofficeTask item)
    {
        var taskStatus = statuses.Single(x => x.Id == item.TaskStatusId);
        return taskStatus.Title.Contains(status, StringComparison.CurrentCultureIgnoreCase);
    }
}

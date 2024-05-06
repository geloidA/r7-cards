using Cardmngr.Domain.Entities;
using Cardmngr.Domain.Enums;
using Cardmngr.Shared.Extensions;

namespace Cardmngr.Reports;

public class OnlyofficeTaskReportData : IOnlyofficeTaskReportData
{
    internal OnlyofficeTaskReportData(OnlyofficeTask task, IEnumerable<OnlyofficeTaskStatus>? statuses = null)
    {
        Title = task.Title;
        var responsibles = string.Join(", ", task.Responsibles.Select(x => x.DisplayName));
        Responsibles = string.IsNullOrWhiteSpace(responsibles) ? "—" : responsibles;
        Deadline = task.Deadline?.ToShortDateString() ?? "—";
        StatusString = GetStatusTitle(task, statuses);
        IsDeadlineOverdue = task.IsDeadlineOut();
    }

    public string Title { get; }
    public string Responsibles { get; }
    public string Deadline { get; }
    public string StatusString { get; }

    public bool IsDeadlineOverdue { get; }

    private static string GetStatusTitle(OnlyofficeTask task, IEnumerable<OnlyofficeTaskStatus>? statuses = null)
    {
        return task.TaskStatusId is null
            ? task.Status == Status.Open ? "Открыта" : "Закрыта"
            : statuses?.Single(x => x.Id == task.TaskStatusId).Title ?? "Неизвестен";
    }
}
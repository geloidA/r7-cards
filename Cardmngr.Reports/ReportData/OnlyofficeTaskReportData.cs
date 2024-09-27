using Cardmngr.Domain.Entities;
using Cardmngr.Domain.Enums;
using Cardmngr.Reports.Base;
using Cardmngr.Shared.Extensions;

namespace Cardmngr.Reports.ReportData;

/// <summary>
/// Данные о задаче Onlyoffice, которые используются для генерации отчета.
/// </summary>
public class OnlyofficeTaskReportData : IOnlyofficeTaskReportData
{

    /// <summary>
    /// Создает экземпляр <see cref="OnlyofficeTaskReportData"/>, используя предоставленную задачу Onlyoffice.
    /// </summary>
    /// <param name="task">Задача Onlyoffice.</param>
    /// <param name="statuses">Статусы задач, которые могут быть использованы.</param>
    public OnlyofficeTaskReportData(OnlyofficeTask task, IEnumerable<OnlyofficeTaskStatus>? statuses = null)
    {
        Title = task.Title;
        var responsibles = string.Join(", ", task.Responsibles.Select(x => x.DisplayName));
        Responsibles = string.IsNullOrWhiteSpace(responsibles) ? "—" : responsibles;
        Deadline = task.Deadline?.ToShortDateString() ?? "—";
        StatusString = GetStatusTitle(task, statuses);
        IsDeadlineOverdue = task.IsDeadlineOut();
    }

    /// <summary>
    /// Название задачи.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Ответственные.
    /// </summary>
    public string Responsibles { get; }

    /// <summary>
    /// Дата дедлайна.
    /// </summary>
    public string Deadline { get; }

    /// <summary>
    /// Статус задачи.
    /// </summary>
    public string StatusString { get; }

    /// <summary>
    /// True, если дедлайн задачи истек.
    /// </summary>
    public bool IsDeadlineOverdue { get; }

    /// <summary>
    /// Получает строку статуса задачи.
    /// </summary>
    /// <param name="task">Задача Onlyoffice.</param>
    /// <param name="statuses">Статусы задач, которые могут быть использованы.</param>
    /// <returns>Строка статуса задачи.</returns>
    private static string GetStatusTitle(OnlyofficeTask task, IEnumerable<OnlyofficeTaskStatus>? statuses = null)
    {
        return task.TaskStatusId is null
            ? task.Status == Status.Open ? "Открыта" : "Закрыта"
            : statuses?.Single(x => x.Id == task.TaskStatusId).Title ?? "Неизвестен";
    }
}

using Onlyoffice.Api.Common;

namespace Cardmngr.Services;

public class FilterManagerService // TODO: Refactor
{
    private readonly object locker = new();
    private readonly Timer timer;
    private bool onlyClosed;
    private bool onlyDeadlined;
    private bool settingsChanged;

    public FilterManagerService()
    {
        // Создаем таймер с интервалом в 1 секунду
        timer = new Timer(TimerCallback, null, Timeout.Infinite, Timeout.Infinite);
    }

    // Метод, вызываемый при изменении настроек
    private void SettingsChanged()
    {
        lock (locker)
        {
            settingsChanged = true;
            // Перезапускаем таймер при каждом изменении настроек
            timer.Change(1000, Timeout.Infinite);
        }
    }

    // Callback метод для таймера
    private void TimerCallback(object? state)
    {
        lock (locker)
        {
            // Если прошло более 1 секунды с последнего изменения настроек
            if (settingsChanged)
            {
                OnFilterChangedInternal();
                settingsChanged = false;
            }
        }
    }

    public event Action<FilterTasksBuilder>? OnFilterChanged;
    private void OnFilterChangedInternal() => OnFilterChanged?.Invoke(GenerateFilter());

    public FilterTasksBuilder GenerateFilter()
    {
        var builder = FilterTasksBuilder.Instance
            .Creator(withCreatedBy)
            .Participant(withResponsible);

        if (onlyClosed) builder = builder.Status(Status.Closed);
        builder = projectId != null ? builder.ProjectId(projectId.Value) : builder.MyProjects(true);

        return onlyDeadlined
            ? builder
                .DeadlineStop(DateTime.Now)
                .Status(Status.Open)
            : builder;
    }

    public bool ToggleClosedFilter()
    {
        onlyClosed = !onlyClosed;
        SettingsChanged();
        return onlyClosed;
    }

    public bool ToggleDeadlineFilter()
    {
        onlyDeadlined = !onlyDeadlined;
        SettingsChanged();
        return onlyDeadlined;
    }

    private string? withResponsible;
    public string? Responsible 
    {
        get => withResponsible;
        set
        {
            withResponsible = value;
            SettingsChanged();
        }
    }

    private string? withCreatedBy;
    public string? CreatedBy
    {
        get => withCreatedBy;
        set
        {
            withCreatedBy = value;
            SettingsChanged();
        }
    }
    
    public string? SetCreatedBy(string? createdByGuid)
    {
        withCreatedBy = createdByGuid;
        SettingsChanged();
        return withCreatedBy;
    }

    private int? projectId;
    public int? ProjectId 
    {
        get => projectId;
        set
        {
            projectId = value;
            SettingsChanged();
        }
    }
}

﻿using Onlyoffice.Api.Common;

namespace Cardmngr.Services;

public class FilterManagerService // TODO: Refactor
{
    private readonly object locker = new();
    private readonly Timer timer;
    private bool onlyClosed;
    private bool onlyDeadlined;
    private string? withCreatedBy;
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

    private FilterTasksBuilder GenerateFilter()
    {
        var builder = FilterTasksBuilder.Instance
            .WithCreator(withCreatedBy)
            .WithParticipant(withResponsible)
            .WithProjectId(projectId);

        if (onlyClosed) builder = builder.WithStatus(Status.Closed);

        return onlyDeadlined
            ? builder
                .WithDeadlineStop(DateTime.Now)
                .WithStatus(Status.Open)
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
﻿using BlazorComponentBus;
using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Components.ProjectAggregate.Models;
using Cardmngr.Domain.Entities;
using Cardmngr.Domain.Entities.Base;
using Cardmngr.Domain.Enums;
using Cardmngr.Shared.Project;
using Cardmngr.Shared.Utils.Filter;
using Cardmngr.Utils;

namespace Cardmngr.Components.ProjectAggregate.States;

public interface IProjectState : IProjectStateViewer
{
    IComponentBus EventBus { get; }
    event Action<EntityChangedEventArgs<Milestone>?>? MilestonesChanged;
    event Action<EntityChangedEventArgs<OnlyofficeTask>?>? TasksChanged;
    event Action? SubtasksChanged;

    bool ReadOnly { get; }

    void AddTask(OnlyofficeTask created);
    void UpdateTask(OnlyofficeTask task);
    void ChangeTaskStatus(OnlyofficeTask task);
    void RemoveTask(OnlyofficeTask taskId);

    void AddMilestone(Milestone milestone);
    void UpdateMilestone(Milestone milestone);
    void RemoveMilestone(Milestone milestoneId);

    void AddSubtask(int taskId, Subtask subtask);
    void UpdateSubtask(Subtask subtask);
    void RemoveSubtask(int taskId, int subtaskId);

    OnlyofficeTaskStatus DefaultStatus(Status status);

    bool Initialized { get; set; }
    void SetModel(ProjectStateDto model);
    Task InitializeTaskTagsAsync(ITaskClient taskClient, bool silent = false, CancellationToken cancellationToken = default);
}

public interface IFilterableProjectState : IProjectState
{
    IFilterManager<OnlyofficeTask> TaskFilter { get; }
}

public interface IRefreshableProjectState : IProjectState
{
    RefreshService RefreshService { get; }
}

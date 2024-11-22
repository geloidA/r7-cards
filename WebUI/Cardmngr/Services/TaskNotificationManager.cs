﻿using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Domain.Entities;
using Cardmngr.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.FluentUI.AspNetCore.Components;
using Onlyoffice.Api.Extensions;
using Onlyoffice.Api.Models.Common;

namespace Cardmngr.Services;

public class TaskNotificationManager(IMessageService messageService, 
    ITaskClient taskClient, 
    AuthenticationStateProvider authentication) 
    : ITaskNotificationManager
{
    public void NotifyDeadline(OnlyofficeTask task)
    {
        messageService.ShowMessageBar(options =>
        {
            options.Intent = MessageIntent.Warning;
            options.Title = "Задача просрочена";
            options.Body = $"Задача \"{task.Title}\" просрочена! В проекте: {task.ProjectOwner.Title}";
            options.Link = new ActionLink<Message>
            {
                Href = $"/project/board?ProjectId={task.ProjectOwner.Id}",
                Text = "Подробнее",
                Target = "_self"
            };
            options.Section = App.MessagesNotificationCenter;
        });
    }

    public void NotifyDeadlines(IEnumerable<OnlyofficeTask> tasks)
    {
        foreach (var task in tasks)
        {
            NotifyDeadline(task);
        }
    }

    public Task NotifyDeadlinesAsync()
    {
        var userId = authentication.ToCookieProvider().UserId ?? throw new InvalidOperationException("User id is null");

        var builder = TaskFilterBuilder.Instance
            .DeadlineOutside()
            .Participant(userId);

        return taskClient
            .GetEntitiesAsync(builder)
            .ForEachAsync(NotifyDeadline);
    }

    public void NotifyNew(OnlyofficeTask task)
    {
        messageService.ShowMessageBar(options =>
        {
            options.Intent = MessageIntent.Info;
            options.Title = "Новая задача";
            options.Body = $"Вам Поручена задача - \"{task.Title}\". В проекте: {task.ProjectOwner.Title}";
            options.Timestamp = DateTime.Now;
            options.Link = new ActionLink<Message>
            {
                Href = $"/project/board?ProjectId={task.ProjectOwner.Id}",
                Text = "Подробнее в проекте",
                Target = "_self"
            };
            options.Section = App.MessagesNotificationCenter;
        });
    }
}

/// <summary>
/// Интерфейс сервиса уведомлений по задачам
/// </summary>
public interface ITaskNotificationManager
{
    /// <summary>
    /// Отправить уведомление о новой задаче
    /// </summary>
    /// <param name="task">Создаваемая задача</param>
    void NotifyNew(OnlyofficeTask task);

    /// <summary>
    /// Отправить уведомление о просроченной задаче
    /// </summary>
    /// <param name="task">Просроченная задача</param>
    void NotifyDeadline(OnlyofficeTask task);

    /// <summary>
    /// Отправить уведомления о просроченных задачах
    /// </summary>
    /// <param name="tasks">Просроченные задачи</param>
    void NotifyDeadlines(IEnumerable<OnlyofficeTask> tasks);

    /// <summary>
    /// Отправить уведомления о просроченных задачах
    /// </summary>
    /// <returns>Задача завершения отправки уведомлений</returns>
    Task NotifyDeadlinesAsync();    
}

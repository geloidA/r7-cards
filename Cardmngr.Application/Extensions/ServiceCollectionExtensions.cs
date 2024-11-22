﻿using Cardmngr.Application.Clients.Feed;
using Cardmngr.Application.Clients.FeedbackClient;
using Cardmngr.Application.Clients.Group;
using Cardmngr.Application.Clients.Milestone;
using Cardmngr.Application.Clients.People;
using Cardmngr.Application.Clients.Project;
using Cardmngr.Application.Clients.Subtask;
using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Application.Clients.TaskStatusClient;
using Cardmngr.Application.Mappers;
using Microsoft.Extensions.DependencyInjection;
using Onlyoffice.Api;
using Onlyoffice.Api.Logics;
using Onlyoffice.Api.Logics.Feed;
using Onlyoffice.Api.Logics.Group;
using Onlyoffice.Api.Logics.Milestone;
using Onlyoffice.Api.Logics.MyTask;
using Onlyoffice.Api.Logics.People;
using Onlyoffice.Api.Logics.Project;
using Onlyoffice.Api.Logics.Repository;
using Onlyoffice.Api.Logics.Subtask;

namespace Cardmngr.Application.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Добавляет клиенты для взаимодействия с Onlyoffice API.
    /// </summary>
    /// <param name="services">Коллекция сервисов, к которой добавляются клиенты.</param>
    /// <returns>Коллекция сервисов с добавленными клиентами.</returns>
    public static IServiceCollection AddApiClients(this IServiceCollection services)
    {
        return services
            .AddScoped<IAuthApiLogic, AuthApiLogic>()
            .AddScoped<IPeopleRepository, ApiPeopleRepository>()
            .AddScoped<IGroupRepository, ApiGroupRepository>()
            .AddScoped<IGroupClient, GroupClient>()
            .AddScoped<IFeedClient, FeedClient>()
            .AddScoped<IFeedRepository, ApiFeedRepository>()
            .AddProjectClient();
    }

    
    /// <summary>
    /// Добавляет клиент для работы с отзывами.
    /// </summary>
    /// <param name="services">Коллекция сервисов, к которой добавляется клиент.</param>
    /// <returns>Коллекция сервисов с добавленным клиентом.</returns>    
    public static IServiceCollection AddFeedbackServices(this IServiceCollection services)
    {
        services.AddScoped<IFeedbackClient, FeedbackClient>();
        return services;
    }
    
    /// <summary>
    /// Добавляет клиенты для работы с проектами, задачами, статусами задач, вехами, подзадачами.
    /// </summary>
    /// <param name="services">Коллекция сервисов, к которой добавляются клиенты.</param>
    /// <returns>Коллекция сервисов с добавленными клиентами.</returns>
    private static IServiceCollection AddProjectClient(this IServiceCollection services)
    {
        return services
            .AddAutoMapper(typeof(EntityMappingProfile), typeof(EnumMappingProfile), typeof(UpdateStatesMappingProfile))
            .AddScoped<IProjectRepository, ApiProjectRepository>()
            .AddScoped<ITaskRepository, ApiTaskRepository>()
            .AddScoped<ITaskStatusRepository, ApiTaskStatusRepository>()
            .AddScoped<IMilestoneRepository, ApiMilestoneRepository>()
            .AddScoped<ISubtaskRepository, ApiSubtaskRepository>()
            .AddScoped<ITaskStatusClient, TaskStatusClient>()
            .AddScoped<IPeopleClient, PeopleClient>()
            .AddScoped<IProjectClient, ProjectClient>()
            .AddScoped<ITaskClient, TaskClient>()
            .AddScoped<ISubtaskClient, SubtaskClient>()
            .AddScoped<IMilestoneClient, MilestoneClient>();
    }
}

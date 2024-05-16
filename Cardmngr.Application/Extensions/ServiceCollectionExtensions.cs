using Cardmngr.Application.Clients;
using Cardmngr.Application.Clients.FeedbackClient;
using Cardmngr.Application.Clients.Milestone;
using Cardmngr.Application.Clients.People;
using Cardmngr.Application.Clients.Subtask;
using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Application.Group;
using Cardmngr.Application.Mappers;
using Microsoft.Extensions.DependencyInjection;
using Onlyoffice.Api;
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
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        return services
            .AddScoped<IPeopleRepository, ApiPeopleRepository>()
            .AddScoped<IFeedbackClient, FeedbackClient>()
            .AddScoped<IGroupRepository, ApiGroupRepository>()
            .AddScoped<IGroupClient, GroupClient>()
            .AddProjectClient();
    }
    
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
            .AddScoped<IUserClient, UserClient>()
            .AddScoped<IProjectClient, ProjectClient>()
            .AddScoped<ITaskClient, TaskClient>()
            .AddScoped<ISubtaskClient, SubtaskClient>()
            .AddScoped<IMilestoneClient, MilestoneClient>();
    }
}

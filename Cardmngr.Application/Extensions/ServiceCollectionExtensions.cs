using Cardmngr.Application.Clients;
using Cardmngr.Application.Clients.FeedbackClient;
using Cardmngr.Application.Clients.Milestone;
using Cardmngr.Application.Clients.Subtask;
using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Application.Mappers;
using Microsoft.Extensions.DependencyInjection;
using Onlyoffice.Api.Logics;
using Onlyoffice.Api.Logics.People;

namespace Cardmngr.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        return services
            .AddScoped<IPeopleApi, PeopleApi>()
            .AddFeedbackClient()
            .AddProjectClient();
    }

    private static IServiceCollection AddFeedbackClient(this IServiceCollection services)
    {
        return services
            .AddScoped<IFeedbackClient, FeedbackClient>();
    }

    private static IServiceCollection AddProjectClient(this IServiceCollection services)
    {
        return services
            .AddAutoMapper(typeof(EntityMappingProfile), typeof(EnumMappingProfile), typeof(UpdateStatesMappingProfile))
            .AddScoped<IProjectApi, ProjectApi>()
            .AddScoped<IProjectClient, ProjectClient>()
            .AddScoped<ITaskClient, TaskClient>()
            .AddScoped<ISubtaskClient, SubtaskClient>()
            .AddScoped<IMilestoneClient, MilestoneClient>();
    }
}

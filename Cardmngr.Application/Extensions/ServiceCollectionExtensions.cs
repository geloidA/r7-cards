using Cardmngr.Application.Clients;
using Microsoft.Extensions.DependencyInjection;
using Onlyoffice.Api.Logics;

namespace Cardmngr.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProjectClient(this IServiceCollection services)
    {
        return services
            .AddAutoMapper(typeof(EntityMappingProfile), typeof(EnumMappingProfile))
            .AddScoped<IProjectApi, ProjectApi>()
            .AddScoped<IProjectClient, ProjectClient>();
    }
}

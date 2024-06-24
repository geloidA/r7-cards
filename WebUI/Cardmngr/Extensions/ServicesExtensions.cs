using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components;
using Onlyoffice.Api.Validations;
using Cardmngr.Shared.Feedbacks;
using Cardmngr.Notification;
using Onlyoffice.Api.Handlers;
using Cardmngr.Validators;
using Cardmngr.Services;
using Cardmngr.Report;
using Blazored.Modal;
using Cardmngr.Utils;

namespace Cardmngr.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddMyCascadingValues(this IServiceCollection services)
    {
        return services
            .AddCascadingValue(sp => new CascadingValueSource<ModalOptions>(
                    "MiddleModal",
                    new ModalOptions { Position = ModalPosition.Middle },
                    true))
            .AddCascadingValue(sp => new CascadingValueSource<ModalOptions>(
                    "DetailsModal",
                    new ModalOptions
                    {
                        Position = ModalPosition.Middle,
                        Size = ModalSize.ExtraLarge,
                        DisableBackgroundCancel = true,
                        UseCustomLayout = true
                    },
                    true))
            .AddCascadingValue(sp => new CascadingValueSource<HeaderInteractionService>(new HeaderInteractionService(), true));
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        return services
            .AddScoped<LoginDataValidator>()
            .AddScoped<FeedbackUpdateDataValidator>()
            .AddScoped<TaskUpdateDataValidator>()
            .AddScoped<MilestoneUpdateDataValidator>();
    }

    public static IServiceCollection AddNotifications(this IServiceCollection services)
    {
        return services
            .AddSingleton<NotificationHubConnection>()
            .AddScoped<NotificationService>()
            .AddScoped<NotificationJSModule>();
    }

    public static IServiceCollection AddProjectsInfo(this IServiceCollection services)
    {
        return services
            .AddSingleton<AppProjectsInfoService>()
            .AddSingleton<IProjectFollowChecker>(x => x.GetRequiredService<AppProjectsInfoService>())
            .AddSingleton<IFollowedProjectManager>(x => x.GetRequiredService<AppProjectsInfoService>());
    }

    public static IServiceCollection AddReports(this IServiceCollection services)
    {
        return services
            .AddScoped<IReportService, ReportService>()
            .AddScoped<ReportJSModule>();
    }

    public static void ConfigureHttpClients(this WebAssemblyHostBuilder builder)
    {
        builder.Services
            .AddHttpClient("onlyoffice", opt => opt.BaseAddress = new Uri($"{builder.HostEnvironment.BaseAddress}onlyoffice/"))
            .AddHttpMessageHandler<CookieHandler>();
    }

    public static IServiceCollection AddCommonServices(this IServiceCollection services)
    {
        return services
            .AddScoped<AppInfoService>()
            .AddScoped<ICircularElementSwitcherService<int>, CircularElementSwitcherService<int>>() // for project tasks switcher in dashboard
            .AddScoped<ITaskNotificationManager, TaskNotificationManager>()
            .AddSingleton<ITagColorManager, TagColorGetter>()
            .AddSingleton<AllProjectsPageSummaryService>()
            .AddTransient<RefreshService>();
    }
}

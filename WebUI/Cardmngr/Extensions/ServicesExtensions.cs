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
using Microsoft.FluentUI.AspNetCore.Components;

namespace Cardmngr.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddMyCascadingValues(this IServiceCollection services)
    {
        return services
            .AddCascadingValue(_ => new CascadingValueSource<ModalOptions>(
                    "MiddleModal",
                    new ModalOptions { Position = ModalPosition.Middle },
                    true))
            .AddCascadingValue(_ => new CascadingValueSource<ModalOptions>(
                    "DetailsModal",
                    new ModalOptions
                    {
                        Position = ModalPosition.Middle,
                        Size = ModalSize.ExtraLarge,
                        DisableBackgroundCancel = true,
                        UseCustomLayout = true
                    },
                    true))
            .AddCascadingValue(_ => new CascadingValueSource<HeaderInteractionService>(new HeaderInteractionService(), true))
            .AddCascadingValue(_ => new CascadingValueSource<TimeAgoOptions>(
                    new TimeAgoOptions
                    {
                        DayAgo = "{0} ден. назад",
                        DaysAgo = "{0} ден. назад",
                        HourAgo = "{0} ч. назад",
                        HoursAgo = "{0} ч. назад",
                        MinuteAgo = "{0} мин. назад",
                        MinutesAgo = "{0} мин. назад",
                        SecondAgo = "{0} сек. назад",
                        SecondsAgo = "{0} сек. назад",
                        MonthAgo = "{0} мес. назад",
                        MonthsAgo = "{0} мес. назад",
                        YearAgo = "{0} г. назад",
                        YearsAgo = "{0} г. назад",
                    },
                    true));
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        return services
            .AddScoped<LoginDataValidator>()
            .AddScoped<FeedbackUpdateDataValidator>()
            .AddScoped<TaskUpdateDataValidator>()
            .AddScoped<ProjectCreateDtoValidator>()
            .AddScoped<MilestoneUpdateDataValidator>();
    }

    public static IServiceCollection AddNotifications(this IServiceCollection services)
    {
        return services
            .AddSingleton<NotificationHubConnection>()
            .AddScoped<NotificationService>()
            .AddScoped<NotificationJsModule>();
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
            .AddScoped<ReportJsModule>();
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
            .AddSingleton<IFeedFilterService, FeedFilterService>()
            .AddSingleton<ITagColorManager, TagColorGetter>()
            .AddSingleton<AllProjectsPageSummaryService>()
            .AddTransient<RefreshService>();
    }
}

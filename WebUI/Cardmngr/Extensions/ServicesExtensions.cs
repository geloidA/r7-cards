using Blazored.Modal;
using Cardmngr.Notification;
using Cardmngr.Report;
using Cardmngr.Reports;
using Cardmngr.Services;
using Cardmngr.Shared.Feedbacks;
using Cardmngr.Validators;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Onlyoffice.Api.Handlers;
using Onlyoffice.Api.Validations;

namespace Cardmngr;

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
                    true));
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

    public static IServiceCollection AddReports(this IServiceCollection services)
    {
        return services
            .AddScoped<TaskReportService>()
            .AddScoped<TaskReportGenerator>()
            .AddScoped<ReportJSModule>();
    }

    public static void ConfigureHttpClients(this WebAssemblyHostBuilder builder)    
    {
        builder.Services
            .AddHttpClient("onlyoffice", opt => opt.BaseAddress = new Uri(builder.Configuration["proxy-url"] 
                ?? throw new NullReferenceException("proxy-url config is null")))
            .AddHttpMessageHandler<CookieHandler>();

        builder.Services
            .AddHttpClient("self-api", opt => opt.BaseAddress = new Uri($"{builder.HostEnvironment.BaseAddress}api"));

        builder.Services
            .AddHttpClient("self-api-cookie", opt => opt.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
            .AddHttpMessageHandler<CookieHandler>();
    }
}

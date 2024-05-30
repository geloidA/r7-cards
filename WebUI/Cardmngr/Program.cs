using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.Modal;
using Cardmngr;
using Onlyoffice.Api.Handlers;
using Onlyoffice.Api.Providers;
using Onlyoffice.Api.Logics;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using Cardmngr.Services;
using Cardmngr.Application.Extensions;
using KolBlazor.Extensions;
using Cardmngr.Utils;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
    .AddScoped<AuthenticationStateProvider, CookieStateProvider>()
    .AddScoped<CookieHandler>()
    .AddScoped<IAuthApiLogic, AuthApiLogic>()
    .AddScoped<AppInfoService>()
    .AddScoped<ITaskNotificationManager, TaskNotificationManager>()
    .AddSingleton<ITagColorManager, TagColorGetter>()
    .AddSingleton<AllProjectsPageSummaryService>()
    .AddTransient<RefreshService>()
    .ConfigureServices()
    .AddBlazoredModal()
    .AddProjectsInfo()
    .AddKolBlazor()
    .AddFluentUIComponents()
    .AddNotifications()
    .AddReports()
    .AddCascadingAuthenticationState()
    .AddBlazoredLocalStorage()
    .AddAuthorizationCore()
    .AddOptions();

builder.Services
    .AddMyCascadingValues()
    .AddValidators();

builder.ConfigureHttpClients();

await builder.Build().RunAsync();

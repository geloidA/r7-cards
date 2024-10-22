using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.Modal;
using Cardmngr;
using Onlyoffice.Api.Handlers;
using Onlyoffice.Api.Providers;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using Cardmngr.Application.Extensions;
using KolBlazor.Extensions;
using Microsoft.FluentUI.AspNetCore.Components;
using Cardmngr.Extensions;
using BlazorComponentBus;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services
    .AddScoped<AuthenticationStateProvider, CookieStateProvider>()
    .AddScoped<CookieHandler>()
    .AddSingleton<IComponentBus, ComponentBus>()
    .AddSingleton<ComponentBus>()
    .AddCommonServices()
    .AddApiClients()
    .AddFeedbackServices()
    .AddBlazoredModal()
    .AddProjectsInfo()
    .AddKolBlazor()
    .AddFluentUIComponents()
    .AddNotifications()
    .AddReports()
    .AddCascadingAuthenticationState()
    .AddBlazoredLocalStorage()
    .AddAuthorizationCore()
    .AddMyCascadingValues()
    .AddValidators()
    .AddOptions();

builder.ConfigureHttpClients();

await builder.Build().RunAsync().ConfigureAwait(false);
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
using KolBlazor.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var config = builder.Configuration;

builder.Services
    .AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) })
    .AddScoped<DragModule>()
    .AddScoped<DragEventModule>()
    .AddScoped<AuthenticationStateProvider, CookieStateProvider>()
    .AddScoped<CookieHandler>()
    .AddScoped<IAuthApiLogic, AuthApiLogic>()
    .AddScoped<IProjectApi, ProjectApi>()
    .AddScoped<TeamMemberSelectionDialog>()
    .AddBlazoredModal()
    .AddKolBlazor()
    .AddBlazoredLocalStorage()
    .AddBlazorBootstrap()
    .AddAuthorizationCore()
    .AddOptions();

builder.Services.AddMyCascadingValues();

builder.Services
    .AddHttpClient("api", opt => opt.BaseAddress = new Uri(config["proxy-url"] 
        ?? throw new NullReferenceException("proxy-url config is null")))
    .AddHttpMessageHandler<CookieHandler>();

await builder.Build().RunAsync();

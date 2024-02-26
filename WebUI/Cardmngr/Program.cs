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
using Cardmngr.Utils.DetailsModal;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
    .AddScoped<AuthenticationStateProvider, CookieStateProvider>()
    .AddScoped<CookieHandler>()
    .AddScoped<IAuthApiLogic, AuthApiLogic>()
    .ConfigureServices()
    .AddSingleton<DetailsModalManager>()
    .AddScoped<TeamMemberSelectionDialog>()
    .AddBlazoredModal()
    .AddKolBlazor()
    .AddBlazoredLocalStorage()
    .AddBlazorBootstrap()
    .AddAuthorizationCore()
    .AddOptions();

builder.Services
    .AddMyCascadingValues()
    .AddValidators();

var config = builder.Configuration;

builder.Services
    .AddHttpClient("onlyoffice", opt => opt.BaseAddress = new Uri(config["proxy-url"] 
        ?? throw new NullReferenceException("proxy-url config is null")))
    .AddHttpMessageHandler<CookieHandler>();

builder.Services.AddHttpClient("self-api", opt => opt.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

builder.Services
    .AddHttpClient("self-api-cookie", opt => opt.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<CookieHandler>();

await builder.Build().RunAsync();

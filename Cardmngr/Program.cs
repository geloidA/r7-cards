using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.Modal;
using Cardmngr;
using Onlyoffice.Api.Handlers;
using Onlyoffice.Api.Providers;
using Onlyoffice.Api.Logics;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Cardmngr.Services;
using KolBlazor;

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
    .AddScoped<CardDropService>()
    .AddBlazoredModal()
    .AddKolBlazor()
    .AddBlazoredLocalStorage()
    .AddBlazorBootstrap()
    .AddAuthorizationCore()
    .AddOptions();

builder.Services
    .AddCascadingValue(sp =>
    {
        var headerTitle = new HeaderTitle();
        return new CascadingValueSource<HeaderTitle>(headerTitle, true);
    })
    .AddCascadingValue(sp =>
    {
        var modalOptions = new ModalOptions { Position = ModalPosition.Middle };
        return new CascadingValueSource<ModalOptions>("MiddleModal", modalOptions, true);
    });

builder.Services
    .AddHttpClient("api", opt => opt.BaseAddress = new Uri(config["proxy-url"] 
        ?? throw new NullReferenceException("proxy-url config is null")))
    .AddHttpMessageHandler<CookieHandler>();

await builder.Build().RunAsync();

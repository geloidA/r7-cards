using Blazored.Modal;
using Bunit;
using Cardmngr.Components;
using Cardmngr.Pages;
using Cardmngr.Services;
using Microsoft.Extensions.DependencyInjection;
using Onlyoffice.Api.Logics;

namespace Cardmngr.Tests.Component.ProjectPageTests;

public class ProjectPageTests : TestContext
{
    public ProjectPageTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services
            .AddBlazoredModal()
            .AddBlazorBootstrap()
            .AddScoped<CardDropService>()
            .AddScoped<IProjectApi, ProjectApiMock>();
    }

    [Fact]
    public void SimpleTest()
    {
        var cut = RenderComponent<ProjectPage>(p => 
            p.Add(p => p.ProjectId, 1)
             .AddCascadingValue(new HeaderTitle()));
        
        var boardView = cut.FindComponent<BoardView>();
        
        Assert.NotNull(boardView);
    }
}

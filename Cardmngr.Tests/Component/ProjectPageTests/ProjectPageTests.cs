using Blazored.Modal;
using Bunit;
using Cardmngr.Components;
using Cardmngr.Models;
using Cardmngr.Pages;
using Cardmngr.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Onlyoffice.Api.Logics;

namespace Cardmngr.Tests.Component.ProjectPageTests;

public class ProjectPageTests : TestContext
{
    public ProjectPageTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        var projectApiMock = new Mock<IProjectApi>();
        Services
            .AddBlazoredModal()
            .AddBlazorBootstrap()
            .AddScoped(_ => projectApiMock.Object);
    }

    [Fact]
    public void SimpleTest()
    {
        var cut = RenderComponent<ProjectPage>(p => 
            p.Add(p => p.ProjectId, 1)
             .AddCascadingValue(new HeaderTitle()));
        
        var boardView = cut.FindComponent<StatusColumnsView>();
        
        Assert.NotNull(boardView);
    }
}

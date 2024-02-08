using BlazorBootstrap;
using Bunit;
using Cardmngr.Components;
using Cardmngr.Components.Modals.DetailModals;
using Cardmngr.Services;
using Cardmngr.Tests.Models;
using Cardmngr.Tests.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Onlyoffice.Api.Logics;
using Onlyoffice.Api.Models;
using STask = System.Threading.Tasks.Task;

namespace Cardmngr.Tests;

public class MilestoneDetailsModalTests : TestContext
{
    private readonly Mock<IProjectApi> projectApiMock = new();
    private readonly Mock<IConfiguration> configurationServiceMock = new();
    private readonly Mock<TeamMemberSelectionDialog> selectionDialogMock = new();

    public MilestoneDetailsModalTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;

        configurationServiceMock.Setup(x => x["proxy-url"]).Returns("https://proxy-url.com");

        Services
            .AddScoped(_ => projectApiMock.Object)
            .AddScoped(_ => selectionDialogMock.Object)
            .AddScoped(_ => configurationServiceMock.Object)
            .AddBlazorBootstrap();
    }

    [Fact]
    public void CreationMode_ShoulShowCorrectInterface()
    {
        var cut = RenderComponent<MilestoneDetailsModal>(p => p
            .Add(p => p.IsCreation, true)
            .Add(p => p.Milestone, ModelCreator.GetMilestone()));

        Assert.DoesNotContain("Удалить", cut.Markup);

        var button = cut.FindComponents<Button>();

        Assert.Contains("Создать", button.Single().Markup);
    }

    [Fact]
    public void Save_ShouldPreventSavingMilestoneWithEmptyTitle()
    {
        var project = new ProjectModelBuilder().Build();

        var milestone = project.Milestones.First();

        milestone.Title = "";
        milestone.Deadline = null;

        var cut = RenderComponent<MilestoneDetailsModal>(p => p
            .Add(p => p.IsCreation, true)
            .Add(p => p.Milestone, milestone));

        cut.Find("form").Submit();

        projectApiMock.Verify(x => x.CreateMilestoneAsync(It.IsAny<int>(), It.IsAny<UpdatedStateMilestone>()), Times.Never());
    }
}

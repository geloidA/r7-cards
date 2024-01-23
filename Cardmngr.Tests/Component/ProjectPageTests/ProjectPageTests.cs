using Bunit;
using Cardmngr.Components;
using Cardmngr.Pages;

namespace Cardmngr.Tests.Component.ProjectPageTests;

public class ProjectPageTests : TestContext
{
    [Fact]
    public void SimpleTest()
    {
        var cut = RenderComponent<ProjectPage>();
        
        var boardView = cut.FindComponent<BoardView>();
        
        Assert.NotNull(boardView);
    }
}

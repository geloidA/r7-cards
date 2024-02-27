using Cardmngr.Application.Clients.SignalRHubClients;
using Cardmngr.Application.Clients.TaskClient;
using Cardmngr.Application.Exceptions;
using Microsoft.AspNetCore.Components;
using Moq;

namespace Cardmngr.Application.Tests;

public class ProjectHubClientTests
{
    private readonly Mock<NavigationManager> navigationManager = new();
    private readonly Mock<ITaskClient> taskClient = new();

    public ProjectHubClientTests()
    {
    }
}
